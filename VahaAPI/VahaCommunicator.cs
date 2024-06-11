using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace VahaAPI
{
    public class VahaCommunicator
    {
        private readonly UdpCommunicator udpCommunicator;
        public VahaModel Vaha { get; set; } = new VahaModel();

        public VahaCommunicator(String ipAddress, int port)
        {
            udpCommunicator = new UdpCommunicator(ipAddress, port);
        }

        public void Close()
        {
            udpCommunicator.Close();
        }

        public void ReadValues()
        {
            string output = "", value = "";
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
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
    }
}
