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
        private TcpClient _client;
        private User _currentuser;
        private string _message;
        private INetworkService _networkservice;
        private SynchronizationContext location = SynchronizationContext.Current;
        private string _internalIP;
        private string _externalIP;
        public ChatViewModel(TcpClient client, User user, INetworkService networkService)
        {
            _currentuser = user;
            _client = client;
            _messages = new ObservableCollection<Message>();
            _networkservice = networkService;
            _internalIP = InternalIP.GetInternallIP();
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

        public User CurrentUser 
        {
            get => _currentuser;
        }
        public string InternalIP => _internalIP;

        public ICommand Send => new RelayCommand(SendMessage, CanSend);
         

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
                        location.Post(delegate
                        {
                            Messages.Clear();
                            foreach (var message in data.MessageCollection)
                                Messages.Add(message);
                        }, null);
                    }
                }catch(IOException e)
                {
                    // No data is found.
                }
            }            
        }
    }
}
