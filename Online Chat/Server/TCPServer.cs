using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using Online_Chat.Models;
using System.Threading.Tasks;

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
            ListenAsync();
        }

        private void ListenAsync()
        {
            _listentask = new Task(async () => 
            {  
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    _clients.Add(client);
                    await Task.Delay(2000);
                    Task.Run(ReadActiveUsers);
                }
            });
            _listentask.Start();
        }

        private void ReadActiveUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            BinaryFormatter bf = new BinaryFormatter();
            foreach (var Client in _clients)
            {
                using (NetworkStream stream = new NetworkStream(Client.Client, false))
                {
                    users.Add((User)bf.Deserialize(stream));               
                }
            }
            BroadCastActiveUsers(users);            
        }
        private void BroadCastActiveUsers(ObservableCollection<User> usersToBroadcast)
        {
            Task.Run(() =>
            {
                BinaryFormatter bf = new BinaryFormatter();
                foreach (var Client in _clients)
                {
                    using (NetworkStream stream = new NetworkStream(Client.Client, false))
                    {
                        bf.Serialize(stream, usersToBroadcast);
                    }
                }
            });     

        }
    }
}
