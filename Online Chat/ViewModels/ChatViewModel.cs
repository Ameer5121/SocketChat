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
        private ObservableCollection<Message> _texts;
        private ObservableCollection<User> _activeusers;
        private TcpClient _client;
        private User _currentuser;
        private INetworkService _networkservice;
        private DispatcherTimer _updateactiveusers;

        public ChatViewModel(TcpClient client, User user, INetworkService networkService)
        {
            _currentuser = user;
            _client = client;
            _texts = new ObservableCollection<Message>();
            _networkservice = networkService;
            _updateactiveusers = new DispatcherTimer();
            _updateactiveusers.Tick += RequestUsers;
            _updateactiveusers.Interval = TimeSpan.FromSeconds(1);
           _updateactiveusers.Start();
        }

        public ObservableCollection<Message> Texts
        {
           get => _texts;
           set => SetPropertyValue(ref _texts, value);
        }

        public ObservableCollection<User> ActiveUsers
        {
            get => _activeusers;
            set => SetPropertyValue(ref _activeusers, value);
        }

         public ICommand Send => new RelayCommand(SendMessage, CanSend);
         

        private bool CanSend()
        {
            return true;
        }

        private void SendMessage()
        {
            using (NetworkStream stream = new NetworkStream(_client.Client, false))
            {
                
            }
        }   
        
        // asnyc void because of DispatcherTimer
        private async void RequestUsers(object sender, EventArgs e) 
        {
            _updateactiveusers.Stop();
            ActiveUsers = await _networkservice.ReceiveUsersAsync(_client);
            _updateactiveusers.Start();
        }
    }
}
