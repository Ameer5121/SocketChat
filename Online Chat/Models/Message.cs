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
        public string Sender => _sender;
        [ProtoMember(2)]
        public string Content => _content;
    }
}
