using System;
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
using System.Net.Http;
using Online_Chat.Views;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Online_Chat.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private User _user;
        private TcpClient _client;
        private TCPServer _listener;
        private string _ipaddress;
        private int _port;        
        private string _status;
        private bool _isconnecting;
        public event EventHandler<MessageEventArgs> Alert;
        public event EventHandler OnConnect;
        public HomeViewModel()
        {
            _client = new TcpClient();
            _user = new User();
            Port = 5000;
            IP = "0.0.0.0";
        }

        public User User => _user;

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

        public ICommand _connect => new RelayCommand(ConnectAsync, CanConnect);
        public ICommand _host => new RelayCommand(Host, CanHost);


        private bool CanConnect()
        {
            if (User.Name != null && !_isconnecting)
                return true;

            return false;
        }

        private async Task ConnectAsync()
        {
            try
            {            
                UpdateStatus("Connecting...");
                _isconnecting = true;
                await Task.WhenAny(Task.Delay(5000), _client.ConnectAsync(IPAddress.Parse(_ipaddress), _port));
                if (!_client.Connected)
                {
                    Alert?.Invoke(this, new MessageEventArgs { Message = "The remote server does not exist!" });
                    UpdateStatus(default);
                    _isconnecting = false;
                    return;
                }
                await Task.Run(SendUser);
                OnConnect?.Invoke(this, EventArgs.Empty);
            }
            catch (FormatException)
            {
               Alert?.Invoke(this, new MessageEventArgs { Message = "Please type a correct IP/Port address!" });
               UpdateStatus(default);
               _isconnecting = false;
            }
            catch (ArgumentException x)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = x.Message });
              UpdateStatus(default);
              _isconnecting = false;
            }
            catch (SocketException y)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = y.Message });
              UpdateStatus(default);
              _isconnecting = false;
            }

        }

        private bool CanHost()
        {
            if (User.Name != null && _isconnecting == false)
                return true;

            return false; 
        }

        private void Host()
        {
            _listener = new TCPServer(new TcpListener(IPAddress.Any, _port));          
            _user.IsHosting = true;
            IP = IP.GetInternallIP();
            ConnectAsync();
        }

        /// <summary>
        ///  Sends the current user to the server
        /// </summary>
        private void SendUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (NetworkStream stream = new NetworkStream(_client.Client, false))
            {
                bf.Serialize(stream, _user);
            }   
        }

        private void UpdateStatus(string status)
        {
            Status = status;
        }

    }
}
