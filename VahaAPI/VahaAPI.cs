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
            if (Vaha == null)
            {
                Vaha = new VahaModel();
            }
            bool hasData = false;
            PropertyInfo[] properties = typeof(VahaModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    string? displayName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    if (displayName != null)
                    {
                        string output = udpCommunicator.SendAndReceive("?" + displayName + "=");
                        if (output.Length == 0)
                        {
                            continue;
                        }

                        string[] delimitted = output.Split('=');
                        string value = delimitted[1].Replace(" ", "");
                        if (value.Length == 0)
                        {
                            continue;
                        }

                        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        if (propertyType == typeof(float))
                        {
                            try
                            {
                                var floatValue = float.Parse(value, CultureInfo.InvariantCulture);
                                property.SetValue(Vaha, floatValue);
                            }
                            catch (Exception)
                            {
                                property.SetValue(Vaha, -10000.0f);
                            }
                        }
                        else if (propertyType == typeof(int))
                        {
                            try
                            {
                                var intValue = int.Parse(value);
                                property.SetValue(Vaha, intValue);
                            }
                            catch (Exception)
                            {
                                property.SetValue(Vaha, -10000);
                            }
                        }
                        else
                        {
                            property.SetValue(Vaha, value);
                        }
                        hasData = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (!hasData)
            {
                Vaha = null;
            }
        }

        public List<string> SetValues(VahaModel vahaValues, bool isControl = false)
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
                        string output = udpCommunicator.SendAndReceive(sendData, isControl);
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

        public bool SetZeroing()
        {
            string output = udpCommunicator.SendAndReceive("!PZ=0");
            string[] delimitted = output.Split('=');
            string receivedValue = delimitted[1].Replace(" ", "");
            return receivedValue == "OK";
        }

        public bool SetTabulkaVazeniRemove(int id)
        {
            bool isControl = true;
            string output = udpCommunicator.SendAndReceive("!NN=" + id, isControl);
            string[] delimitted = output.Split('=');
            string receivedValue = delimitted[1].Replace(" ", "");
            return receivedValue == "OK";
        }

        public bool SetActualDateAndTime()
        {
            try
            {
                string output = udpCommunicator.SendAndReceive("!TM=" + DateTime.Now.ToString("HH.mm.ss"));
                string[] delimitted = output.Split('=');
                string receivedValue = delimitted[1].Replace(" ", "");
                if (receivedValue == "OK")
                {
                    output = udpCommunicator.SendAndReceive("!DT=" + DateTime.Now.ToString("dd.MM.yyyy"));
                    delimitted = output.Split('=');
                    receivedValue = delimitted[1].Replace(" ", "");
                    return receivedValue == "OK";
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
