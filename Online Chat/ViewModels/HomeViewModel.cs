using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Input;
using Online_Chat.Models;
using Online_Chat.Command;
using Online_Chat.Server;
using Online_Chat.Events;
using System.Net.Http;

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
        public event EventHandler<MessageEventArgs> Alert;
        public event EventHandler OnConnectOrHost;
        public HomeViewModel()
        {
            _client = new TcpClient();
            _user = new User();           
            IP = "0.0.0.0";
        }

        public User User => _user;

        public TcpClient Client => _client;
        public string IP
        {
            get => _ipaddress;
            set => SetPropertyValue(ref _ipaddress, value);

        }
        public int Port
        {
            get => _port;
            set => SetPropertyValue(ref _port, value);
        } 
        private string Status
        {
            get => _status;
            set => SetPropertyValue(ref _status, value);
        }

        public ICommand _connect => new RelayCommand(Connect, CanConnect);
        public ICommand _host => new RelayCommand(Host);


        private bool CanConnect()
        {
            return true;
        }

        private async void Connect()
        { 
            try
            {
                
                 UpdateStatus("Connecting...");
                _client.ConnectAsync(IPAddress.Parse(_ipaddress), _port);
                await Task.Delay(5000);
                if (!_client.Connected)
                {
                    Alert?.Invoke(this, new MessageEventArgs { Message = "The remote server does not exist!" });
                    UpdateStatus(default);
                    return;
                }
                OnConnectOrHost?.Invoke(this, EventArgs.Empty);

            }
            catch (FormatException)
            {
               Alert?.Invoke(this, new MessageEventArgs { Message = "Please type a correct IP/Port address!" });
            }
            catch (ArgumentException x)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = x.Message });
            }
            catch (SocketException y)
            {
              Alert?.Invoke(this, new MessageEventArgs { Message = y.Message });
            }          
        }
        private async void Host()
        {
            try
            {
                _user.IsHosting = true;
                _listener = new TCPServer(new TcpListener(IPAddress.Any, _port));
                IP = await GetIP();
                Connect();             
            }
            catch (ArgumentException)
            {
                Alert.Invoke(this, new MessageEventArgs { Message = "Please type a correct Port address"});
            }
        }

        private async Task<string> GetIP()
        {
            WebClient web = new WebClient();
            string externalip = await web.DownloadStringTaskAsync("http://icanhazip.com/");
            return externalip.Substring(0, externalip.Length - 2);
        }

        private void UpdateStatus(string status)
        {
            Status = status;
        }

    }
}
