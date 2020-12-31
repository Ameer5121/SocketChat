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
using System.Threading.Tasks;
using Online_Chat.Extensions;
namespace Online_Chat.ViewModels
{
    class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Message> _texts;
        private ObservableCollection<User> _activeusers;
        private TcpClient _client;
        private User _user;
        public event EventHandler OnInitialize;

        public ChatViewModel(TcpClient client, User user)
        {
            _user = user;
            _client = client;
            _texts = new ObservableCollection<Message>();
            InitializeUsersAsync();
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

         public ICommand _send => new RelayCommand(SendMessage, CanSend);

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

        private async Task InitializeUsersAsync()
        {
            ActiveUsers = await Task.Run(ReceiveUsers);
            OnInitialize?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Recieves users from the server when logged in
        /// </summary>
        /// <param name="Users"></param>
        private Task<ObservableCollection<User>> ReceiveUsers()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (NetworkStream stream = new NetworkStream(_client.Client, false))
            {
                return Task.FromResult((ObservableCollection<User>)bf.Deserialize(stream));
            }
        }
    }
}
