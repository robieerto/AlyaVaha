using AlyaVaha.DAL.Repositories;
using AlyaVaha.Models;
using Photino.NET;
using System.Text.Json;

namespace AlyaVaha
{
    public static class MainMessageHandler
    {
        static public void MessageHandler(object sender, string message)
        {
            var window = (PhotinoWindow)sender;
            object? responseValue = null;
            WindowCommand? windowCommand = null;

            try
            {
                if (message == null || message.Length == 0)
                {
                    return;
                }

                windowCommand = JsonSerializer.Deserialize<WindowCommand>(message);

                if (windowCommand == null)
                {
                    return;
                }

                switch (windowCommand.Command)
                {
                    case "SetValues":
                        DataCommunicator.CommandQueue.Enqueue(windowCommand);
                        break;
                    case "SetControlValues":
                        DataCommunicator.CommandQueue.Enqueue(windowCommand);
                        break;
                    case "SetZeroing":
                        DataCommunicator.CommandQueue.Enqueue(windowCommand);
                        break;
                    case "GetNavazovania":
                        responseValue = NavazovanieRepository.GetList();
                        break;
                    case "GetZariadenia":
                        responseValue = ZariadenieRepository.GetList();
                        break;
                    case "GetMaterialy":
                        responseValue = MaterialRepository.GetList();
                        break;
                    case "GetZasobniky":
                        responseValue = ZasobnikRepository.GetList();
                        break;
                    case "GetCesty":
                        responseValue = CestaRepository.GetList();
                        break;
                    case "AddMaterial":
                        if (windowCommand.Value != null)
                        {
                            var material = JsonSerializer.Deserialize<Material>(windowCommand.Value);
                            if (material != null)
                            {
                                MaterialRepository.Add(material);
                            }
                        }
                        responseValue = "Materiál bol pridaný";
                        break;
                    case "UpdateMaterial":
                        if (windowCommand.Value != null)
                        {
                            var material = JsonSerializer.Deserialize<Material>(windowCommand.Value);
                            if (material != null)
                            {
                                MaterialRepository.Update(material);
                            }
                        }
                        responseValue = "Materiál bol upravený";
                        break;
                    case "DeleteMaterial":
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            MaterialRepository.Delete(id);
                        }
                        responseValue = "Materiál bol zmazaný";
                        break;
                    case "AddZasobnik":
                        if (windowCommand.Value != null)
                        {
                            var zasobnik = JsonSerializer.Deserialize<Zasobnik>(windowCommand.Value);
                            if (zasobnik != null)
                            {
                                ZasobnikRepository.Add(zasobnik);
                            }
                        }
                        responseValue = "Zásobník bol pridaný";
                        break;
                    case "UpdateZasobnik":
                        if (windowCommand.Value != null)
                        {
                            var zasobnik = JsonSerializer.Deserialize<Zasobnik>(windowCommand.Value);
                            if (zasobnik != null)
                            {
                                ZasobnikRepository.Update(zasobnik);
                            }
                        }
                        responseValue = "Zásobník bol upravený";
                        break;
                    case "DeleteZasobnik":
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            ZasobnikRepository.Delete(id);
                        }
                        responseValue = "Zásobník bol zmazaný";
                        break;
                    case "AddNavazovanie":
                        if (windowCommand.Value != null)
                        {
                            var navazovanie = JsonSerializer.Deserialize<Navazovanie>(windowCommand.Value);
                            if (navazovanie != null)
                            {
                                NavazovanieRepository.Add(navazovanie);
                            }
                        }
                        responseValue = "Navažovanie bolo pridané";
                        break;
                    case "UpdateNavazovanie":
                        if (windowCommand.Value != null)
                        {
                            var navazovanie = JsonSerializer.Deserialize<Navazovanie>(windowCommand.Value);
                            if (navazovanie != null)
                            {
                                NavazovanieRepository.Update(navazovanie);
                            }
                        }
                        responseValue = "Navažovanie bolo upravené";
                        break;
                    case "DeleteNavazovanie":
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            NavazovanieRepository.Delete(id);
                        }
                        responseValue = "Navažovanie bolo zmazané";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                responseValue = ex.Message;
            }

            if (windowCommand != null && responseValue != null)
            {
                WindowCommand response = new WindowCommand(windowCommand.Command, JsonSerializer.Serialize(responseValue));
                window?.SendWebMessage(JsonSerializer.Serialize(response));
            }
        }
    }
}
