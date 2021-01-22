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
    public class SerializationData
    {
        private ObservableCollection<User> _usercollection;
        private ObservableCollection<Message> _messagecollection;

        [ProtoMember(1)]
        public ObservableCollection<User> UserCollection
        {
            get => _usercollection;
            set => _usercollection = value;
        }
        [ProtoMember(2)]
        public ObservableCollection<Message> MessageCollection
        {
            get => _messagecollection;
            set => _messagecollection = value;
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
