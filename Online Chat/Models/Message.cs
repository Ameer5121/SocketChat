using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract]
    public class Message
    {
        private string _sender;
        private string _content;

        [ProtoMember(1)]
        public string Sender
        {           
            get => _sender;
            set => _sender = value;
        }
        [ProtoMember(2)]
        public string Content
        {
            get => _content;
            set => _content = value;
        }
    }
}
