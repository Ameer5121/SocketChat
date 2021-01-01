using Online_Chat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;

namespace Online_Chat.Services
{
    class NetworkService : INetworkService
    {

        public Task<ObservableCollection<Message>> ReceiveMessages()
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<ObservableCollection<User>> ReceiveUsers(TcpClient client)
        {
            return await Task.Run(() =>
             {
                 BinaryFormatter bf = new BinaryFormatter();
                 using (NetworkStream stream = new NetworkStream(client.Client, false))
                 {
                     return Task.FromResult((ObservableCollection<User>)bf.Deserialize(stream));
                 }
             });
            
        }
    }
}
