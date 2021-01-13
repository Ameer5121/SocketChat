using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Online_Chat.Models;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Online_Chat.Services
{
    public interface INetworkService
    {
        Task<ObservableCollection<Message>> ReceiveMessagesAsync();
        Task<ObservableCollection<User>> ReceiveUsersAsync(TcpClient client);
    }
}
