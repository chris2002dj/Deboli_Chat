using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ChatLibrary.Model
{
    public class NicknameClass
    {
        public string Nickname { get; set; }
        public TcpClient Client { get; set; }
    }
}
