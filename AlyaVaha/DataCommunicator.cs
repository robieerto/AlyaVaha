using Photino.NET;
using System.Text.Json;

namespace AlyaVaha
{
    public static class DataCommunicator
    {
        private static VahaAPI.VahaAPI? vahaCommunicator { get; set; }
        private static PhotinoWindow? Window { get; set; }

        public static Queue<WindowCommand> CommandQueue { get; set; } = new Queue<WindowCommand>();

        public static void Init(PhotinoWindow window)
        {
            Window = window;
            InitVahaCommunicator();
        }

        private static void InitVahaCommunicator()
        {
            vahaCommunicator = new VahaAPI.VahaAPI("192.168.1.10", 3396);
        }

        public static async Task Run()
        {
            await Task.Delay(100);

            while (true)
            {
                try
                {
                    var command = CommandQueue.Dequeue();
                    if (command != null)
                    {
                        switch (command.Command)
                        {
                            case "SetValues":
                                var vahaModel = JsonSerializer.Deserialize<VahaAPI.VahaModel>(command.Value);
                                if (vahaModel != null)
                                {
                                    vahaCommunicator?.SetValues(vahaModel);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    vahaCommunicator?.ReadValues();
                    // Send values to frontend
                    Window?.SendWebMessage(JsonSerializer.Serialize(vahaCommunicator?.Vaha));
                    Console.WriteLine(JsonSerializer.Serialize(vahaCommunicator?.Vaha.BruttoVaha));
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
