using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract] [ProtoInclude(100, typeof(User))]
    public class SerializationData
    {
    }
}
