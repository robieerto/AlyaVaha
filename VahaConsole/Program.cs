using System.Text.Json;
using VahaAPI;

var vahaCommunicator = new VahaCommunicator("192.168.1.10", 3396);

while (true)
{
    try
    {
        await Task.Delay(100);
        vahaCommunicator.ReadValues();
        Console.WriteLine(JsonSerializer.Serialize(vahaCommunicator.Vaha.BruttoVaha));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
