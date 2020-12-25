using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Online_Chat.Server
{
    public class TCPServer
    {
        private TcpListener _server;
        private List<TcpClient> _clients;
        private Task _listentask;
        private byte[] _data;

        public TcpListener Server => _server;
        public TCPServer(TcpListener listener)
        {
            _clients = new List<TcpClient>();
            _data = new byte[256];
            _server = listener;
            _server.Start();
            Listen();
        }

        private void Listen()
        {
            _listentask = new Task(() => 
            {  
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    _clients.Add(client);
                }
            });
        }

        private void Read()
        {
            foreach (var Client in _clients)
            {

            }
        }

        ~TCPServer()
        {
            System.Windows.MessageBox.Show("Disposed of the server");
        }
    }
}
