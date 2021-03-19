using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Bassini_AsyncSocketLib
{
    public class AsyncSocketClient
    {
        IPAddress mServerIpAddress;
        int mServerPort;
        TcpClient mClient;

        public IPAddress ServerIpAddress {
            get
            {
                return mServerIpAddress;
            }
        }
        public int ServerPort
        {
            get
            {
                return mServerPort;
            }
        }
        public bool SetServerIPAddress(string str_IPAddress)
        {
            IPAddress ipaddr = null;
            if (!IPAddress.TryParse(str_IPAddress,out ipaddr))
            {
                Console.WriteLine("Ip non valido.");
                return false;
            }

            mServerIpAddress = ipaddr;
            return true;
        }
        public bool SetServerPort(string str_port)
        {
            int port = -1;
            if(!int.TryParse(str_port,out port))
            {
                Console.WriteLine("Porta non valida");
                return false;
            }
            if(port < 0 || port > 65535)
            {
                Console.WriteLine("La porta deve essere compressa tra 0 e 65535");
                return false;
            }
            mServerPort = port;
            return true;
        }

        public async Task ConnettiAlServer()
        {
            if (mClient == null)
            {
                mClient = new TcpClient();
            }

            try
            {
                await mClient.ConnectAsync(mServerIpAddress, mServerPort);
                Console.WriteLine("Connesso al server IP/Port: {0} / {1}",
                                    mServerIpAddress.ToString(),mServerPort);
                RiceviMessaggi();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        private async Task RiceviMessaggi()
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = mClient.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];
                int nBytes = 0;

                while (true)
                {
                    nBytes = await reader.ReadAsync(buff, 0, buff.Length);
                    if (nBytes == 0){
                        Console.WriteLine("Disconnesso.");
                        break;
                    }
                    string recvMessage = new string(buff, 0, nBytes);
                    Console.WriteLine(recvMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Invia(string messaggio)
        {
            if (mClient == null)
            {
                return;
            }
            if (!mClient.Connected)
            {
                return;
            }

            try
            {
                byte[] buff = Encoding.ASCII.GetBytes(messaggio);
                mClient.GetStream().WriteAsync(buff, 0, buff.Length);
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public AsyncSocketClient()
        {
            mServerIpAddress = null;
            mServerPort = -1;
            mClient = null;
        }


    }
}
