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
        public Task<T> ReceiveDataAsync<T>(TcpClient client)            
        {
            using (NetworkStream stream = new NetworkStream(client.Client, false))
            {
                stream.ReadTimeout = 3000;
                return Task.FromResult(Serializer.DeserializeWithLengthPrefix<T>(stream, PrefixStyle.Fixed32));
            }
        }
    }
}
