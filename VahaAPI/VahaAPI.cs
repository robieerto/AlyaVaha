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
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    string? displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    if (displayName != null)
                    {
                        string output = udpCommunicator.SendAndReceive("?" + displayName + "=");

                        string[] delimitted = output.Split('=');
                        string value = delimitted[1].Replace(" ", "");
                        if (value.Length == 0)
                        {
                            continue;
                        }

                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        if (propertyType == typeof(float))
                        {
                            var floatValue = float.Parse(value, CultureInfo.InvariantCulture);
                            property.SetValue(Vaha, floatValue);
                        }
                        else if (propertyType == typeof(int))
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public List<string> SetValues(VahaModel vahaValues)
        {
            var notSetValues = new List<string>();
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    string? displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    object? value = property.GetValue(vahaValues);
                    if (value != null && displayName != null)
                    {
                        string sendData = "!" + displayName + "=" + value.ToString();
                        string output = udpCommunicator.SendAndReceive(sendData);
                        string[] delimitted = output.Split('=');
                        string receivedValue = delimitted[1].Replace(" ", "");
                        if (receivedValue.Length == 0 || receivedValue != "OK")
                        {
                            notSetValues.Add(property.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return notSetValues;
        }
    }
}
