using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Online_Chat.Models
{
    
    class SerializationData
    {
       public enum Objects
       {
          User,
          Message,
       }
       public enum Collections
       {
            UserCollection,
            MessageCollection,
       }
    }
}
