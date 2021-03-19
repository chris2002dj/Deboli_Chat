using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Bassini_AsyncSocketLib
{
    public class AsyncSocketServer
    {
        IPAddress mIP;
        int mPort;
        TcpListener mServer;
        List<NicknameModel> mClients;

        public AsyncSocketServer()
        {
            mClients = new List<NicknameModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <param name="port"></param>
        public async void InAscolto(IPAddress ipaddr = null, int port = 23000)
        {
            //controlli generali
            if (ipaddr == null)
            {
                ipaddr = IPAddress.Any;
            }
            if (port < 0 || port > 65535)
            {
                port = 23000;
            }

            mIP = ipaddr;
            mPort = port;

            mServer = new TcpListener(mIP, mPort);

            Console.WriteLine("Server in ascolto su IP: {0} - Porta: {1}", mIP.ToString(), mPort.ToString());

            mServer.Start();

            Console.WriteLine("Server avviato.");
            while (true)
            {
                TcpClient client = await mServer.AcceptTcpClientAsync();

                RegistraClient(client);

                //Console.WriteLine("Client Connessi: {0}. Client Connesso: {1}", mClients.Count, client.Client.RemoteEndPoint);

                RiceviMessaggio(client);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public async void RegistraClient(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];
                int nBytes = 0;

                Console.WriteLine("In attesa di un nickname");

                //ricezione nickname asincrono
                nBytes = await reader.ReadAsync(buff, 0, buff.Length);
                string recvText = new string(buff);
                Console.WriteLine("N° byte: {0}. Nickname: {1}", nBytes, recvText);


                NicknameModel newClient = new NicknameModel();
                newClient.Nickname = recvText;
                newClient.Client = client;

                mClients.Add(newClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public async void RiceviMessaggio(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];
                int nBytes = 0;
                while (true)
                {
                    Console.WriteLine("In attesa di un messaggio");

                    // Ricezione messaggio asincrono
                    nBytes = await reader.ReadAsync(buff, 0, buff.Length);
                    if (nBytes == 0)
                    {
                        RimuoviClient(client);
                        Console.WriteLine("Client disconnesso");
                        break;
                    }
                    string recvText = new string(buff);
                    Console.WriteLine("N° byte: {0}. Messaggio: {1}", nBytes, recvText);
                    InviaATutti(recvText);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void RimuoviClient(TcpClient client)
        {
            // Metodo Standard
            foreach (NicknameModel elem in mClients)
            {
                if (elem.Client == client) {
                    mClients.Remove(elem);
                }
            }

            // LINQ
            {
                /* LINQ - Modo C#
                NicknameModel nm = mClients.Where(riga => riga.Client == client).FirstOrDefault();

                if (nm != null) {
                    mClients.Remove(nm);
                }

                // LINQ - Mode SQL
                NicknameModel nm2 = (from elemento in mClients
                                     where elemento.Client == client
                                     select elemento).FirstOrDefault();

                if (nm2 != null) {
                    mClients.Remove(nm2);
                }
                */
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messaggio"></param>
        public void InviaATutti(string messaggio)
        {
            try
            {
                foreach (NicknameModel client in mClients)
                {
                    byte[] buff = Encoding.ASCII.GetBytes(messaggio);
                    client.Client.GetStream().WriteAsync(buff, 0, buff.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnetti()
        {
            try
            {
                foreach (NicknameModel client in mClients)
                {
                    client.Client.Close();
                    RimuoviClient(client.Client);
                }
                mServer.Stop();
                
                
            }
            catch (Exception ex)
            {

                Console.WriteLine("Errore: "+ ex.Message);
            }

        }

    }
}