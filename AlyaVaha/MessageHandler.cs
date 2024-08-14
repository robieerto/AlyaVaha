using AlyaLibrary;
using AlyaVaha.DAL.Repositories;
using AlyaVaha.Models;
using DevExtreme.AspNet.Data;
using Photino.NET;
using System.Text.Json;

namespace AlyaVaha
{
    public static class MainMessageHandler
    {
        static public JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

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
                    case "Login":
                        responseValue = new OperationResult("Nesprávne prihlasovacie údaje", false);
                        var loginData = JsonSerializer.Deserialize<Uzivatel>(windowCommand.Value!, jsonOptions);
                        if (loginData != null && loginData.Login != null && loginData.Heslo != null)
                        {
                            var uzivatel = UzivatelRepository.Authenticate(loginData.Login, loginData.Heslo);
                            if (uzivatel != null)
                            {
                                var zariadenie = ZariadenieRepository.GetList()?.Find(z => z.Id == loginData.Id);
                                if (zariadenie != null)
                                {
                                    DataCommunicator.InitVahaCommunicator(zariadenie);
                                    DataCommunicator.LoggedInUzivatel = uzivatel;
                                }
                                responseValue = new OperationResult("Prihlásenie bolo úspešné", true, JsonSerializer.Serialize(uzivatel));
                            }
                        }
                        break;
                    case "GetLoggedInUser":
                        responseValue = DataCommunicator.LoggedInUzivatel;
                        responseValue ??= "null";
                        break;
                    case "GetSelectedZariadenie":
                        responseValue = DataCommunicator.SelectedZariadenie ?? ZariadenieRepository.GetList().FirstOrDefault();
                        responseValue ??= "null";
                        break;
                    case "Logout":
                        DataCommunicator.CloseVahaCommunicator();
                        DataCommunicator.LoggedInUzivatel = null;
                        responseValue = new OperationResult("Odhlásenie bolo úspešné", true);
                        break;
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
                    case "GetExportNavazovania":
                    case "GetStatistiky":
                        var loadOptions = JsonSerializer.Deserialize<DataSourceLoadOptionsBase>(windowCommand.Value!, jsonOptions);
                        responseValue = NavazovanieRepository.GetList(loadOptions!);
                        if (loadOptions != null && loadOptions.Take == 0)
                        {
                            var propertyInfo = responseValue.GetType().GetProperty("groupCount");
                            propertyInfo?.SetValue(responseValue, 0);
                        }
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
                    case "GetUzivatelia":
                        responseValue = UzivatelRepository.GetList();
                        break;
                    case "AddMaterial":
                        responseValue = new OperationResult("Materiál nebol pridaný", false);
                        if (windowCommand.Value != null)
                        {
                            var material = JsonSerializer.Deserialize<Material>(windowCommand.Value);
                            if (material != null)
                            {
                                responseValue = MaterialRepository.Add(material);
                            }
                        }
                        break;
                    case "UpdateMaterial":
                        responseValue = new OperationResult("Materiál nebol upravený", false);
                        if (windowCommand.Value != null)
                        {
                            var material = JsonSerializer.Deserialize<Material>(windowCommand.Value);
                            if (material != null)
                            {
                                responseValue = MaterialRepository.Update(material);
                            }
                        }
                        break;
                    case "DeleteMaterial":
                        responseValue = new OperationResult("Materiál nebol zmazaný", false);
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            responseValue = MaterialRepository.Delete(id);
                        }
                        break;
                    case "AddZasobnik":
                        responseValue = new OperationResult("Zásobník nebol pridaný", false);
                        if (windowCommand.Value != null)
                        {
                            var zasobnik = JsonSerializer.Deserialize<Zasobnik>(windowCommand.Value);
                            if (zasobnik != null)
                            {
                                responseValue = ZasobnikRepository.Add(zasobnik);
                            }
                        }
                        break;
                    case "UpdateZasobnik":
                        responseValue = new OperationResult("Zásobník nebol upravený", false);
                        if (windowCommand.Value != null)
                        {
                            var zasobnik = JsonSerializer.Deserialize<Zasobnik>(windowCommand.Value);
                            if (zasobnik != null)
                            {
                                responseValue = ZasobnikRepository.Update(zasobnik);
                            }
                        }
                        break;
                    case "DeleteZasobnik":
                        responseValue = new OperationResult("Zásobník nebol zmazaný", false);
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            responseValue = ZasobnikRepository.Delete(id);
                        }
                        break;
                    case "DeleteNavazovanie":
                        responseValue = new OperationResult("Navažovanie nebolo zmazané", false);
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            responseValue = NavazovanieRepository.Delete(id);
                        }
                        break;
                    case "DeleteNavazovaniaByFilter":
                        responseValue = new OperationResult("Navažovania neboli zmazané", false);
                        if (windowCommand.Value != null)
                        {
                            loadOptions = JsonSerializer.Deserialize<DataSourceLoadOptionsBase>(windowCommand.Value, jsonOptions);
                            responseValue = NavazovanieRepository.DeleteByFilter(loadOptions!);
                        }
                        break;
                    case "UpdateZariadenie":
                        responseValue = new OperationResult("Zariadenie nebolo upravené", false);
                        if (windowCommand.Value != null)
                        {
                            var zariadenie = JsonSerializer.Deserialize<Zariadenie>(windowCommand.Value);
                            if (zariadenie != null)
                            {
                                responseValue = ZariadenieRepository.Update(zariadenie);
                                DataCommunicator.InitVahaCommunicator(zariadenie);
                            }
                        }
                        break;
                    case "AddUzivatel":
                        responseValue = new OperationResult("Užívateľ nebol pridaný", false);
                        if (windowCommand.Value != null)
                        {
                            var uzivatel = JsonSerializer.Deserialize<Uzivatel>(windowCommand.Value);
                            if (uzivatel != null)
                            {
                                responseValue = UzivatelRepository.Add(uzivatel);
                            }
                        }
                        break;
                    case "UpdateUzivatel":
                        responseValue = new OperationResult("Užívateľ nebol upravený", false);
                        if (windowCommand.Value != null)
                        {
                            var uzivatel = JsonSerializer.Deserialize<Uzivatel>(windowCommand.Value);
                            if (uzivatel != null)
                            {
                                responseValue = UzivatelRepository.Update(uzivatel);
                            }
                        }
                        break;
                    case "DeleteUzivatel":
                        responseValue = new OperationResult("Užívateľ nebol zmazaný", false);
                        if (windowCommand.Value != null)
                        {
                            int id = JsonSerializer.Deserialize<int>(windowCommand.Value);
                            responseValue = UzivatelRepository.Delete(id);
                        }
                        break;
                    case "LogError":
                        if (windowCommand.Value != null)
                        {
                            Library.WriteClientLog(windowCommand.Value);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
                responseValue = new OperationResult(ex.Message, false);
            }

            try
            {
                if (windowCommand != null && responseValue != null)
                {
                    WindowCommand response = new WindowCommand(windowCommand.Command, JsonSerializer.Serialize(responseValue));
                    window?.SendWebMessage(JsonSerializer.Serialize(response));
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog(ex);
            }
        }
    }
}
