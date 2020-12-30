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
namespace Online_Chat.ViewModels
{
    class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<string> _texts;
        private ObservableCollection<string> _activeusers;
        private TcpClient _client;
        private User _user;

        public ChatViewModel(TcpClient client, User user)
        {
            _user = user;
            _client = client;
            _texts = new ObservableCollection<string>();
            QueryUsers();
        }

        public ObservableCollection<string> Texts => _texts;

        public ObservableCollection<string> ActiveUsers
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

        private async Task QueryUsers()
        {
           _activeusers = await Task.Run(ReceiveUsers);
        }
        /// <summary>
        /// Recieves users from the server when logged in
        /// </summary>
        /// <param name="Users"></param>
        private Task<ObservableCollection<string>> ReceiveUsers()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (NetworkStream stream = new NetworkStream(_client.Client, false))
            {
              return Task.FromResult((ObservableCollection<string>)bf.Deserialize(stream));
            }
        }
    }
}
