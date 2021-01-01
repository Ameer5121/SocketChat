using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Chat.ViewModels;

namespace Online_Chat.Events
{
    public class ConnectEventArgs : EventArgs
    {
        public ChatViewModel ChatVM;
    }
}
