using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Online_Chat.Models;
using System.Net.Sockets;
using System.Net;

namespace Online_Chat.Services
{
    interface INetworkService
    {
        Task<ObservableCollection<Message>> ReceiveMessages();
        Task<ObservableCollection<User>> ReceiveUsers(TcpClient client);
    }
}
