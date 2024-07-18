using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VahaAPI
{
    public class UdpCommunicator
    {
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;

        public readonly string ipAddress;
        public readonly int port;

        public UdpCommunicator(string remoteAddress, int remotePort)
        {
            try
            {
                ipAddress = remoteAddress;
                port = remotePort;
                udpClient = new UdpClient();
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteAddress), remotePort);
                udpClient.Connect(remoteEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send(string message)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length);
        }

        public string Receive()
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            return Encoding.UTF8.GetString(data);
        }

        public string SendAndReceive(string message, bool isControl = false)
        {
            string output = "";
            bool repeat = true;
            while (repeat)
            {
                repeat = false;
                try
                {
                    Send(message);
                    var scheduledReconnect = isControl ? scheduleLightReconnect() : scheduleReconnect();
                    output = Receive();
                    scheduledReconnect.Cancel();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.GetType() == typeof(SocketException))
                    {
                        repeat = true;
                    }
                }
            }

            return output;
        }

        public void Reconnect()
        {
            Close();
            udpClient = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            udpClient.Connect(remoteEndPoint);
        }

        public void Close()
        {
            udpClient.Close();
        }

        private CancellationTokenSource scheduleReconnect()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Delay(100).ContinueWith(async (t) =>
            {
                Console.WriteLine("Reopen communication");
                Reconnect();
            }, cancellationToken);
            return cancellationTokenSource;
        }

        private CancellationTokenSource scheduleLightReconnect()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Delay(1000).ContinueWith(async (t) =>
            {
                Console.WriteLine("Reopen communication");
                Reconnect();
            }, cancellationToken);
            return cancellationTokenSource;
        }
    }
}
