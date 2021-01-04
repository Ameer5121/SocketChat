using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Online_Chat.ViewModels;
using System.Collections.ObjectModel;
using Online_Chat.Models;
using System.Threading.Tasks;

namespace Online_Chat.Server
{
    public class TCPServer
    {
        private TcpListener _server;
        private List<TcpClient> _clients;
        private ObservableCollection<User> users;
        private Task _listentask;

        public TcpListener Server => _server;
        public TCPServer(TcpListener listener)
        {
            _clients = new List<TcpClient>();
            users = new ObservableCollection<User>();
            _server = listener;
            _server.Start();
            ListenForConnections();
        }

        private void ListenForConnections()
        {
            _listentask = new Task(() => 
            {  
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    _clients.Add(client);
                    ReadActiveUser();
                }
            });
            _listentask.Start();
        }

        private void ReadActiveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (NetworkStream stream = new NetworkStream(_clients.Last().Client, false))
            {
               users.Add((User)bf.Deserialize(stream));
            }
            BroadCastActiveUser(users);
        }
        private void BroadCastActiveUser(ObservableCollection<User> usersToBroadcast)
        {
            BinaryFormatter bf = new BinaryFormatter();
            foreach (var Client in _clients)
            {
                using (NetworkStream stream = new NetworkStream(Client.Client, false))
                {
                    bf.Serialize(stream, usersToBroadcast);
                }
            }
        }
    }
}
