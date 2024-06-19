using AlyaVaha.DAL.Repositories;
using Photino.NET;
using System.Text.Json;

namespace AlyaVaha
{
    public static class MainMessageHandler
    {
        static public void MessageHandler(object sender, string message)
        {
            var window = (PhotinoWindow)sender;

            if (message == null || message.Length == 0)
            {
                return;
            }

            WindowCommand? windowCommand = JsonSerializer.Deserialize<WindowCommand>(message);

            if (windowCommand == null)
            {
                return;
            }

            object? responseValue = null;

            switch (windowCommand.Command)
            {
                case "SetValues":
                    DataCommunicator.CommandQueue.Enqueue(windowCommand);
                    break;
                case "GetNavazovania":
                    responseValue = NavazovanieRepository.GetList();
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
                default:
                    break;
            }

            if (responseValue != null)
            {
                WindowCommand response = new WindowCommand(windowCommand.Command, JsonSerializer.Serialize(responseValue));
                window?.SendWebMessage(JsonSerializer.Serialize(response));
            }
        }
    }
}
