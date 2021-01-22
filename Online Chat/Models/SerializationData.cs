using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ProtoBuf;

namespace Online_Chat.Models
{
    [ProtoContract]
    public readonly struct SerializationData
    {
        [ProtoMember(1)]
        public readonly ObservableCollection<User> UserCollection;
        [ProtoMember(2)]
        public readonly ObservableCollection<Message> MessageCollection;

        public SerializationData(ObservableCollection<User> usercollection, ObservableCollection<Message> messagecollection)
        {
            UserCollection = usercollection;
            MessageCollection = messagecollection;
        }

        //Byte Identification 
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
