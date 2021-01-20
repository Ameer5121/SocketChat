using Online_Chat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;
using ProtoBuf;

namespace Online_Chat.Services
{
    class NetworkService : INetworkService
    {
        public async Task<ObservableCollection<T>> ReceiveDataAsync<T>(TcpClient client) 
            where T: SerializationData
        {
            return await Task.Run(() =>
            {
                using (NetworkStream stream = new NetworkStream(client.Client, false))
                {
                    return Task.FromResult(Serializer.DeserializeWithLengthPrefix<ObservableCollection<T>>(stream, PrefixStyle.Fixed32));
                }
            });
        }
    }
}
