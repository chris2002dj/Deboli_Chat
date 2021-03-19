using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bassini_AsyncSocketLib;

namespace Deboli_Chat_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncSocketServer server = new AsyncSocketServer();
            server.InAscolto();
            Console.ReadLine();
        }
    }
}
