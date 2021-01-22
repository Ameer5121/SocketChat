using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Online_Chat.ViewModels;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract]
    public struct User
    {
        [ProtoMember(1)]
        public string Name { get; }
        [ProtoMember(2)]
        public bool IsHosting { get; }

        public User(string name, bool ishosting)
        {
            Name = name;
            IsHosting = ishosting;
        }
       
    }
}
