using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Chat.Models
{
    class Message
    {
        private string _sender;
        private string _content;

        public string Sender => _sender;
        public string Content => _content;
    }
}
