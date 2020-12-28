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

namespace Online_Chat.ViewModels
{
    class ChatViewModel
    {
        private ObservableCollection<string> _texts;
        private ObservableCollection<User> _activeusers;
        private TcpClient _client;
        private User _user;

        public ChatViewModel(TcpClient client, User user)
        {
            _user = user;
            _client = client;
            _texts = new ObservableCollection<string>();
            _activeusers = new ObservableCollection<User>();
            ReceiveUsers();
        }

        public ObservableCollection<string> Texts => _texts;

        public ObservableCollection<User> ActiveUsers
        {
            get => _activeusers;
        }

         public ICommand _send => new RelayCommand(SendMessage, CanSend);

        private bool CanSend()
        {
            return true;
        }

        private void SendMessage()
        {
            using (NetworkStream stream = _client.GetStream())
            {
                
            }
        }     

        /// <summary>
        /// Recieves users from the server
        /// </summary>
        /// <param name="Users"></param>
        private void ReceiveUsers()
        {
            BinaryFormatter bf = new BinaryFormatter();
            IEnumerable<string> usersToReceive;
            using (NetworkStream stream = _client.GetStream())
            {
                usersToReceive = (List<string>)bf.Deserialize(stream);
            }
        }
    }
}
