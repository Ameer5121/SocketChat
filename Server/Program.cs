using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Program
    {
        private static TcpListener _server;
        static void Main(string[] args)
        {
            _server = new TcpListener(IPAddress.Loopback, 500);
            _server.Start();
            Listen();   
        }

        private static void Listen()
        {
            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
            }
        }
        
    }
}
