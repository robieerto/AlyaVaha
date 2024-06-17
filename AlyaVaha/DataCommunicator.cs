using Photino.NET;
using System.Text.Json;

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
            vahaAPI = new VahaAPI.VahaAPI("192.168.1.10", 3396);
        }

        public static async Task Run()
        {
            await Task.Delay(1000);

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
                            switch (command.Command)
                            {
                                case "SetValues":
                                    var vahaModel = JsonSerializer.Deserialize<VahaAPI.VahaModel>(command.Value);
                                    if (vahaModel != null)
                                    {
                                        vahaAPI?.SetValues(vahaModel);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Read values from Vaha
                    vahaAPI?.ReadValues();
                    // Send values to frontend
                    Window?.SendWebMessage(JsonSerializer.Serialize(vahaAPI?.Vaha));
                    Console.WriteLine(JsonSerializer.Serialize(vahaAPI?.Vaha.BruttoVaha));
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
