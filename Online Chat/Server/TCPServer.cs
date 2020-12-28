using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

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
            _listentask = new Task(async () => 
            {  
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    _clients.Add(client);
                    await Task.Delay(2000);
                    ReadActiveUsers();
                }
            });
        }

        private void ReadActiveUsers()
        {
            List<string> users = new List<string>();
            foreach(var Client in _clients)
            {
                using (NetworkStream stream = Client.GetStream())
                {
                    byte[] userName = new byte[256];
                    stream.Read(userName, 0, userName.Length);
                    users.Add(Encoding.ASCII.GetString(userName));                  
                }
            }
            BroadCastActiveUsers(users);            
        }
        private void BroadCastActiveUsers(IEnumerable<string> usersToBroadcast)
        {
            BinaryFormatter bf = new BinaryFormatter();
            foreach (var Client in _clients)
            {
                using (NetworkStream stream = Client.GetStream())
                {
                    bf.Serialize(stream, usersToBroadcast);
                }
            }

        }
        ~TCPServer()
        {
            System.Windows.MessageBox.Show("Disposed of the server");
        }
    }
}
