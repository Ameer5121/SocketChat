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
    public class User
    {
        private string _name;
        private bool _ishosting;
        [ProtoMember(1)]
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        [ProtoMember(2)]
        public bool IsHosting
        {
            get => _ishosting;
            set => _ishosting = value;
        }
    }
}
