using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract]
    public struct Message
    {
        [ProtoMember(1)]
        public string Sender { get; }
        [ProtoMember(2)]
        public string Content { get; }


        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
