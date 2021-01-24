using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net;
using System.Windows.Input;
using Online_Chat.Models;
using Online_Chat.Command;
using Online_Chat.Server;
using Online_Chat.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using Online_Chat.Extensions;
using Online_Chat.Services;
using ProtoBuf;
namespace Online_Chat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Message> _messages;
        private ObservableCollection<User> _activeusers;
        private List<TcpClient> stresstestusers;
        private TcpClient _client;
        private User _currentuser;
        private string _message;
        private INetworkService _networkservice;

        public ChatViewModel(TcpClient client, User user, INetworkService networkService)
        {
            _currentuser = user;
            _client = client;
            _messages = new ObservableCollection<Message>();
            stresstestusers = new List<TcpClient>();
            _networkservice = networkService;
            Task.Run(ReadData);
        }

        public ObservableCollection<Message> Messages
        {
           get => _messages;
           set => SetPropertyValue(ref _messages, value);
        }

        public ObservableCollection<User> ActiveUsers
        {
            get => _activeusers;
            set => SetPropertyValue(ref _activeusers, value);
        }

         public string Message 
         {
            get => _message;
            set => SetPropertyValue(ref _message, value);
         }

         public ICommand Send => new RelayCommand(SendMessage, CanSend);
         public ICommand Stress => new RelayCommand(StressTest);


        private bool CanSend()
        {
            if (Message == default)
                return false;
            
            return true;
        }

        private void SendMessage()
        {
           using (NetworkStream stream = new NetworkStream(_client.Client, false))
           {
                Message message = new Message(_currentuser.Name, Message);
                Serializer.SerializeWithLengthPrefix(stream, SerializationData.Objects.Message, PrefixStyle.Fixed32);
                Serializer.SerializeWithLengthPrefix(stream, message, PrefixStyle.Fixed32);
                Message = default;
           }
        }   
        
        private async Task ReadData() 
        {
            while (true)             
            {
                try
                {
                    var datatype = await _networkservice.ReceiveDataAsync<SerializationData.Collections>(_client);
                    SerializationData data;
                    if (datatype is SerializationData.Collections.UserCollection)
                    {
                        data = await _networkservice.ReceiveDataAsync<SerializationData>(_client);
                        ActiveUsers = data.UserCollection;
                    }
                    else if (datatype is SerializationData.Collections.MessageCollection)
                    {
                        data = await _networkservice.ReceiveDataAsync<SerializationData>(_client);
                        Messages = data.MessageCollection;
                    }
                }catch(IOException e)
                {
                    // No data is found.
                }
            }            
        }

        private async Task StressTest()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < 500; i++)
                {
                    TcpClient client = new TcpClient();
                    stresstestusers.Add(client);
                    await client.ConnectAsync(IPAddress.Parse("192.168.14.15"), 5000);
                    using (NetworkStream stream = new NetworkStream(client.Client, false))
                    {
                        Serializer.SerializeWithLengthPrefix(stream, SerializationData.Objects.User, PrefixStyle.Fixed32);
                        Serializer.SerializeWithLengthPrefix(stream, new User(i.ToString(), false), PrefixStyle.Fixed32);
                    }
                }
            });
        }
    }
}
