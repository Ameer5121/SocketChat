using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Online_Chat.ViewModels;
using Online_Chat.Models;
using Online_Chat.Extensions;
using System.Collections.ObjectModel;


namespace Online_Chat.Server
{
    public class TCPServer
    {
        private TcpListener _server;
        private  List<TcpClient> _clients;
        private ObservableCollection<User> _users;
        private DispatcherTimer _checkforinactiveusers;

        public TcpListener Server => _server;
        public TCPServer(TcpListener listener)
        {
            _clients = new List<TcpClient>();
            _users = new ObservableCollection<User>();
            _server = listener;
            _server.Start();
            _checkforinactiveusers = new DispatcherTimer();
            _checkforinactiveusers.Tick += LookForInActiveUsers;
            _checkforinactiveusers.Interval = TimeSpan.FromSeconds(5);
            _checkforinactiveusers.Start();
            Task.Run(ListenForConnections);
        }

        private void ListenForConnections()
        {
            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
                _clients.Add(client);
                ReadActiveUser();
            }
        }

        // async void because of DispatcherTimer
        private async void LookForInActiveUsers(object sender, EventArgs e)
        {
            _checkforinactiveusers.Stop();
            await Task.Run(() =>
            {
                var tempcollection = _clients.ToList();
                foreach (var client in tempcollection)
                {
                    using (NetworkStream stream = new NetworkStream(client.Client, false))
                    {
                        try
                        {
                            byte[] empty = { };
                            stream.Write(empty, 0, empty.Length);
                        }
                        catch (IOException x)
                        {
                            var index = _clients.IndexOf(client);
                            _clients.RemoveAt(index);
                            _users.RemoveAt(index);
                        }
                    }
                }
                // Check whether something has been removed from the collecton.
                if (tempcollection.Count != _clients.Count)
                {
                    BroadCastActiveUsers(_users);
                }
            });
            _checkforinactiveusers.Start();
        }

        private void ReadActiveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (NetworkStream stream = new NetworkStream(_clients.Last().Client, false))
            {
               _users.Add((User)bf.Deserialize(stream));
            }
            BroadCastActiveUsers(_users);
        }

        private void BroadCastActiveUsers(ObservableCollection<User> usersToBroadcast)
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
