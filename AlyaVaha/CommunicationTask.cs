using Photino.NET;
using System.Text.Json;
using VahaAPI;

namespace AlyaVaha
{
    public static class CommunicationTask
    {
        public static async Task Run(PhotinoWindow window)
        {
            var vahaCommunicator = new VahaCommunicator("192.168.1.10", 3396);
            await Task.Delay(100);

            while (true)
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                _ = Task.Delay(500).ContinueWith(async (t) =>
                {
                    Console.WriteLine("Reopen communication");
                    vahaCommunicator.Close();
                    vahaCommunicator = new VahaCommunicator("192.168.1.10", 3396);
                }, cancellationToken);

                try
                {
                    await Task.Delay(10);
                    vahaCommunicator.ReadValues();
                    window.SendWebMessage(JsonSerializer.Serialize(vahaCommunicator.Vaha));
                    Console.WriteLine(JsonSerializer.Serialize(vahaCommunicator.Vaha.BruttoVaha));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                cancellationTokenSource.Cancel();
            }
        }
    }
}
