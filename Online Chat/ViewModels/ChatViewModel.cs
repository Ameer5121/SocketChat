using System;
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
namespace Online_Chat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Message> _messages;
        private ObservableCollection<User> _activeusers;
        private TcpClient _client;
        private User _currentuser;
        private INetworkService _networkservice;

        public ChatViewModel(TcpClient client, User user, INetworkService networkService)
        {
            _currentuser = user;
            _client = client;
            _messages = new ObservableCollection<Message>();
            _networkservice = networkService;
            Task.Run(ReadData);
        }

        public ObservableCollection<Message> Texts
        {
           get => _messages;
           set => SetPropertyValue(ref _messages, value);
        }

        public ObservableCollection<User> ActiveUsers
        {
            get => _activeusers;
            set => SetPropertyValue(ref _activeusers, value);
        }

         public string Message { get; set; }

         public ICommand Send => new RelayCommand(SendMessage, CanSend);
         

        private bool CanSend()
        {
            return true;
        }

        private void SendMessage()
        {
           
        }   
        
        private async Task ReadData() 
        {
            while (true)
            {
                // TO DO


                /*await Task.Delay(1000);
                SerializationData data = await _networkservice.ReceiveDataAsync<SerializationData>(_client);
                if (data is SerializationData.User)
                {
                    _activeusers = await _networkservice.ReceiveDataAsync<ObservableCollection<User>>(_client);
                }
                else if (data is SerializationData.Message)
                {
                    //_messages.Add(data as Message);
                }*/
            }
            
        }
    }
}
