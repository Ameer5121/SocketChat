using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract]
    public readonly struct Message
    {
        [ProtoMember(1)]
        public readonly string Sender;
        [ProtoMember(2)]
        public readonly string Content;


        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
