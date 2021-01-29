using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Input;
using Online_Chat.Models;
using Online_Chat.Command;
using Online_Chat.Server;
using Online_Chat.Events;
using Online_Chat.Extensions;
using Online_Chat.Services;
using System.Net.Http;
using Online_Chat.Views;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;

namespace Online_Chat.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private User _user;
        private TcpClient _client;
        private TCPServer _listener;
        private string _username;
        private string _ipaddress;
        private int _port;        
        private string _status;
        private bool _isconnecting;
        public event EventHandler<MessageEventArgs> Alert;
        public event EventHandler<ConnectEventArgs> OnSuccessfulConnect;
        private INetworkService _networkService;
        public HomeViewModel()
        {
            _networkService = new NetworkService();
            _client = new TcpClient();
            Port = 5000;
            IP = "0.0.0.0";
        }

        public string UserName 
        {
            get => _username;
            set => _username = value;
        }

        public TcpClient Client => _client;
        public string IP
        {
            get => _ipaddress;
            set
            {
                if (_isconnecting) return;
               SetPropertyValue(ref _ipaddress, value);
            }

        }

        public int Port
        {
            get => _port;
            set 
            {
                if (value < 5000 || value > 10000 || _isconnecting)
                    return;
                    
                SetPropertyValue(ref _port, value);
            }
        } 
        public string Status
        {
            get => _status;
            set => SetPropertyValue(ref _status, value);
        }

        public ICommand _connect => new RelayCommand(InitiateConnection, CanConnect);
        public ICommand _host => new RelayCommand(Host, CanHost);


        private bool CanConnect()
        {
            if (_username != null && !_isconnecting)
                return true;

            return false;
        }

        private async Task InitiateConnection()
        {
            try
            {            
                UpdateStatus("Connecting...", true);
                await Task.WhenAny(Task.Delay(5000), _client.ConnectAsync(IPAddress.Parse(_ipaddress), _port));
                if (!_client.Connected)
                {
                    Alert?.Invoke(this, new MessageEventArgs { Message = "The remote server does not exist!" });
                    UpdateStatus(default, false);
                    _isconnecting = false;
                    return;
                }
               if(_user.Name == null)
                    CreateUser(false);
                await Task.Run(SendUser);
                OnSuccessfulConnect?.Invoke(this, new ConnectEventArgs { ChatVM = await ConstructChatAsync()});
            }
            catch (FormatException)
            {
               Alert?.Invoke(this, new MessageEventArgs { Message = "Please type a correct IP/Port address!" });
               UpdateStatus(default, false);
            }
            catch (ArgumentException x)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = x.Message });
              UpdateStatus(default, false);
            }
            catch (SocketException y)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = y.Message });
              UpdateStatus(default, false);
            }
        }

        private bool CanHost()
        {
            if (_username != null && _isconnecting == false)
                return true;

            return false; 
        }

        private void Host()
        {
            _listener = new TCPServer(new TcpListener(IPAddress.Any, _port), _networkService);
            CreateUser(true);
            IP = IP.GetInternallIP();
            InitiateConnection();
        }

        /// <summary>
        ///  Sends the current user to the server on a successful connect
        /// </summary>
        private void SendUser()
        {
            using (NetworkStream stream = new NetworkStream(_client.Client, false))
            {               
                Serializer.SerializeWithLengthPrefix(stream, SerializationData.Objects.User, PrefixStyle.Fixed32);
                Serializer.SerializeWithLengthPrefix(stream, _user, PrefixStyle.Fixed32);
            }
        }

        /// <summary>s
        /// If connection is successful, the Chat window will be constructed
        /// </summary>
        /// <param name="networkservice"></param>
        /// <returns></returns>
        private async Task<ChatViewModel> ConstructChatAsync()
        {
            var users = await Task.Run(async () => 
            {
                var datatype = await _networkService.ReceiveDataAsync<SerializationData.Collections>(_client);
                var data = await _networkService.ReceiveDataAsync<SerializationData>(_client);
                return data.UserCollection;
            });
            var ChatVM = new ChatViewModel(_client, _user, _networkService);
            ChatVM.ActiveUsers = users;
            return ChatVM;
        }

        private void UpdateStatus(string status, bool connectingstatus)
        {
            Status = status;
            _isconnecting = connectingstatus;
        }

        private void CreateUser(bool ishosting) => _user = new User(_username, ishosting);
    }
}
