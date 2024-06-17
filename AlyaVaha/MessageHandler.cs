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

            switch (windowCommand.Command)
            {
                case "SetValues":
                    DataCommunicator.CommandQueue.Enqueue(windowCommand);
                    break;
                default:
                    break;
            }
        }
    }
}
