﻿using System;
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
using ProtoBuf;


namespace Online_Chat.Server
{
    public class TCPServer
    {
        private TcpListener _server;
        private List<TcpClient> _clients;
        private ObservableCollection<User> _users;
        private ObservableCollection<Message> _messages;

        public TcpListener Server => _server;
        public TCPServer(TcpListener listener)
        {
            _clients = new List<TcpClient>();
            _users = new ObservableCollection<User>();
            _messages = new ObservableCollection<Message>();
            _server = listener;
            _server.Start();
            Task.Run(ListenForConnections);
            Task.Run(LookForInActiveUsers);
            Task.Run(ReadData);
        }

        private void ListenForConnections()
        {
            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
                _clients.Add(client);
            }
        }

        private async Task LookForInActiveUsers()
        {
            while (true)
            {
                await Task.Delay(1000);
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
                    BroadCastData(_users);
                }
            }
        }
        private async Task ReadData()
        {
            while (true)
            {
                await Task.Delay(1000);
                foreach (var client in _clients)
                {
                    using (NetworkStream stream = new NetworkStream(client.Client, false))
                    {
                        object data = Serializer.DeserializeWithLengthPrefix<object>(stream, PrefixStyle.Fixed32);
                        if (data.GetType() == typeof(User))
                        {
                            _users.Add(data as User);
                            BroadCastData(_users);
                        }else if(data.GetType() == typeof(Message))
                        {
                            _messages.Add(data as Message);
                            BroadCastData(_messages);
                        }
                    }
                }
                
            }
        }
        private void BroadCastData<TData>(ObservableCollection<TData> data)
        {
            foreach (var Client in _clients)
            {
                using (NetworkStream stream = new NetworkStream(Client.Client, false))
                {
                    Serializer.SerializeWithLengthPrefix(stream, data, PrefixStyle.Fixed32);
                }
            }
        }
    }
}
