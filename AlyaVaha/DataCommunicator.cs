﻿using AlyaLibrary;
using AlyaVaha.DAL.Repositories;
using AlyaVaha.Models;
using Photino.NET;
using System.Text.Json;
using VahaAPI;

namespace AlyaVaha
{
    public static class DataCommunicator
    {
        private static VahaAPI.VahaAPI? vahaAPI { get; set; }
        private static PhotinoWindow? Window { get; set; }

        public static Uzivatel? LoggedInUzivatel { get; set; }
        public static Zariadenie? SelectedZariadenie { get; set; }
        public static Queue<WindowCommand> CommandQueue { get; set; } = new Queue<WindowCommand>();
        public static int Timeout { get; set; }
        public static int LightTimeout { get; set; }
        public static bool ReadAllHistoryData { get; set; }

        public static void Init(PhotinoWindow window, int timeout, int lightTimeout, bool readAllHistoryData)
        {
            Window = window;
            Timeout = timeout;
            LightTimeout = lightTimeout;
            ReadAllHistoryData = readAllHistoryData;
        }

        public static void InitVahaCommunicator(Zariadenie zariadenie)
        {
            vahaAPI = new VahaAPI.VahaAPI(zariadenie.IpAdresa!, zariadenie.Port ?? 0, Timeout, LightTimeout);
            SelectedZariadenie = zariadenie;
        }

        public static void CloseVahaCommunicator()
        {
            vahaAPI = null;
        }

        public static async Task Run()
        {
            while (true)
            {
                await Task.Delay(1000);

                if (vahaAPI == null)
                {
                    continue;
                }

                // Send actual date and time in device
                bool? dateTimeSet = false;
                do
                {
                    if (vahaAPI == null)
                    {
                        break;
                    }

                    try
                    {
                        dateTimeSet = vahaAPI?.SetActualDateAndTime();
                    }
                    catch (Exception ex)
                    {
                        Library.WriteLog(ex);
                    }
                } while (!dateTimeSet ?? true);

                while (true)
                {
                    if (vahaAPI == null)
                    {
                        break;
                    }

                    try
                    {
                        // Execute commands from frontend
                        if (CommandQueue.Count > 0)
                        {
                            var command = CommandQueue.Dequeue();
                            if (command != null)
                            {
                                object? responseValue = null;
                                switch (command.Command)
                                {
                                    case "SetValues":
                                        if (command.Value != null)
                                        {
                                            var vahaValues = JsonSerializer.Deserialize<VahaAPI.VahaModel>(command.Value);
                                            if (vahaValues != null)
                                            {
                                                responseValue = vahaAPI?.SetValues(vahaValues);
                                            }
                                        }
                                        break;
                                    case "SetControlValues":
                                        if (command.Value != null)
                                        {
                                            bool isControl = true;
                                            var vahaValues = JsonSerializer.Deserialize<VahaAPI.VahaModel>(command.Value);
                                            if (vahaValues != null)
                                            {
                                                responseValue = vahaAPI?.SetValues(vahaValues, isControl);
                                            }
                                        }
                                        break;
                                    case "SetZeroing":
                                        responseValue = vahaAPI?.SetZeroing();
                                        break;
                                    case "ResetTabulky":
                                        responseValue = vahaAPI?.ResetTabulky();
                                        break;
                                    default:
                                        break;
                                }

                                if (responseValue != null)
                                {
                                    WindowCommand response = new WindowCommand(command.Command, JsonSerializer.Serialize(responseValue));
                                    Window?.SendWebMessage(JsonSerializer.Serialize(response));
                                }
                            }
                        }

                        // Read values from Vaha
                        vahaAPI?.ReadValues();
                        VahaModel? vahaData = vahaAPI?.Vaha;
                        if (vahaData != null)
                        {
                            // If there is new vazenie, save it
                            if (vahaData.TabulkaVazeni != null && vahaData.TabulkaVazeni.Length > 13)
                            {
                                // Parse vazenie and save to database
                                try
                                {
                                    var vazenie = NavazovanieParser.Parse(vahaData.TabulkaVazeni);
                                    var externalId = vazenie.Id;
                                    vazenie.Id = 0;
                                    vazenie.ZariadenieId = SelectedZariadenie?.Id ?? 1;
                                    NavazovanieRepository.Add(vazenie);
                                    ZariadenieRepository.UpdateStatistiky((int)vazenie.ZariadenieId!, vazenie.NavazeneMnozstvo, vazenie.NavazenyPocetDavok);
                                    if (ReadAllHistoryData)
                                    {
                                        // Delete only current vazenie
                                        vahaAPI?.SetTabulkaVazeniRemove(externalId);
                                    }
                                    else
                                    {
                                        // Delete all vazenia
                                        vahaAPI?.SetTabulkaVazeniRemove(-1);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Library.WriteLog(ex);
                                }
                            }

                            // Send values to frontend
                            WindowCommand windowCommand = new WindowCommand("ActualData", JsonSerializer.Serialize(vahaData));
                            Window?.SendWebMessage(JsonSerializer.Serialize(windowCommand));

                            //Console.WriteLine(JsonSerializer.Serialize(vahaData.BruttoVaha));
                        }


                    }
                    catch (Exception ex)
                    {
                        Library.WriteLog(ex);
                    }

                    await Task.Delay(10);
                }
            }
        }
    }
}
