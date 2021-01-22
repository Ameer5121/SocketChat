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
    public readonly struct User
    {
        [ProtoMember(1)]
        public readonly string Name;
        [ProtoMember(2)]
        public readonly bool IsHosting;

        public User(string name, bool ishosting)
        {
            Name = name;
            IsHosting = ishosting;
        }
       
    }
}
