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
            ipAddress = remoteAddress;
            port = remotePort;
            udpClient = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteAddress), remotePort);
            udpClient.Connect(remoteEndPoint);
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

        public string SendAndReceive(string message)
        {
            Send(message);
            return Receive();
        }

        public void Reconnect()
        {
            Close();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            udpClient.Connect(remoteEndPoint);
        }

        public void Close()
        {
            udpClient.Close();
        }
    }
}
