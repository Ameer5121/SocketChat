using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Online_Chat.ViewModels;

namespace Online_Chat.Models
{
    [Serializable]
    public class User
    {
        private string _name;
        private bool _ishosting;

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public bool IsHosting
        {
            get => _ishosting;
            set => _ishosting = value;
        }
    }
}
