using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net;
using System.Windows.Input;
using Online_Chat.Models;
using Online_Chat.Command;
using Online_Chat.Server;
using Online_Chat.Events;

namespace Online_Chat.ViewModels
{
    class ChatViewModel
    {
        private ObservableCollection<string> _texts;
        private TcpClient _client;
        private User _user;

        public ChatViewModel(TcpClient client, User user)
        {
            _user = user;
            _client = client;
            _texts = new ObservableCollection<string>();
            
        }

        public ObservableCollection<string> Texts
        {
            get => _texts;
        }

       // public ICommand _send => new RelayCommand(Send, CanSend);
        
        private bool CanSend()
        {
            return true;
        }

        private void Send()
        {
            using (NetworkStream stream = _client.GetStream())
            {

            }
        }
    }
}
