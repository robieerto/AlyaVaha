using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace VahaAPI
{
    public class VahaAPI
    {
        private readonly UdpCommunicator udpCommunicator;
        public VahaModel Vaha { get; set; } = new VahaModel();

        public VahaAPI(String ipAddress, int port)
        {
            udpCommunicator = new UdpCommunicator(ipAddress, port);
        }

        public void ReadValues()
        {
            string output = "", value = "";
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string? displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                if (displayName != null)
                {
                    udpCommunicator.Send("?" + displayName + "=");
                    output = udpCommunicator.Receive();
                    string[] delimitted = output.Split('=');
                    value = delimitted[1].Replace(" ", "");
                    if (value.Length == 0)
                    {
                        continue;
                    }

                    if (property.PropertyType == typeof(float))
                    {
                        var floatValue = float.Parse(value, CultureInfo.InvariantCulture);
                        property.SetValue(Vaha, floatValue);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        var intValue = int.Parse(value);
                        property.SetValue(Vaha, intValue);
                    }
                    else
                    {
                        property.SetValue(Vaha, value);
                    }
                }
            }
        }

        public void SetValues(VahaModel inputVahaModel)
        {
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(inputVahaModel);
                if (value != null)
                {
                    string? displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    if (displayName != null)
                    {
                        udpCommunicator.Send("!" + displayName + "=" + value);
                    }
                }
            }
        }

        private CancellationTokenSource scheduleReconnect()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            _ = Task.Delay(500).ContinueWith(async (t) =>
            {
                Console.WriteLine("Reopen communication");
                udpCommunicator.Reconnect();
            }, cancellationToken);
            return cancellationTokenSource;
        }
    }
}
