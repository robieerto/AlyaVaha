using AlyaVaha.DAL.Repositories;
using Photino.NET;
using System.Text.Json;
using VahaAPI;

namespace AlyaVaha
{
    public static class DataCommunicator
    {
        private static VahaAPI.VahaAPI? vahaAPI { get; set; }
        private static PhotinoWindow? Window { get; set; }

        public static Queue<WindowCommand> CommandQueue { get; set; } = new Queue<WindowCommand>();

        public static void Init(PhotinoWindow window)
        {
            Window = window;
            InitVahaCommunicator();
        }

        private static void InitVahaCommunicator()
        {
            var zariadenie = ZariadenieRepository.GetList()[0];
            if (zariadenie != null)
            {
                vahaAPI = new VahaAPI.VahaAPI(zariadenie.IpAdresa!, zariadenie.Port ?? 0);
            }
            else
            {
                vahaAPI = new VahaAPI.VahaAPI("192.168.1.10", 3396);
            }
        }

        public static async Task Run()
        {
            await Task.Delay(1000);

            // Send actual date and time in device
            bool? dateTimeSet = false;
            do
            {
                try
                {
                    dateTimeSet = vahaAPI?.SetActualDateAndTime();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (!dateTimeSet ?? true);

            while (true)
            {
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
                                vazenie.ZariadenieId = 1;
                                NavazovanieRepository.Add(vazenie);
                                vahaAPI?.SetTabulkaVazeniRemove(externalId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        // Send values to frontend
                        WindowCommand windowCommand = new WindowCommand("ActualData", JsonSerializer.Serialize(vahaData));
                        Window?.SendWebMessage(JsonSerializer.Serialize(windowCommand));

                        Console.WriteLine(JsonSerializer.Serialize(vahaData.BruttoVaha));
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(10);
            }
        }
    }
}
