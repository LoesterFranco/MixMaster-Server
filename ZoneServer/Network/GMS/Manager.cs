using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace ZoneServer.Network.GMS
{

    public class Manager
    {
        private const string SERVER_IP = "127.0.0.1";
        private const int SERVER_PORT = 30186;

        public Socket socket;
        public const int BUFFER_SIZE = 4096;
        public byte[] buffer;

        private Receive ReceiveManager;
        private Send SendManager;

        public Manager()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[BUFFER_SIZE];
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);
            ReceiveManager = new Receive();
            SendManager = new Send();
            socket.BeginConnect(ip, new AsyncCallback(ConnectCallback), socket);
        }

        private void ConnectCallback(IAsyncResult e)
        {
            Socket s = (Socket)e.AsyncState;
            try
            {
                s.EndConnect(e);
                Init.logger.ConsoleLog("[GMS] Connectado com sucesso!", ConsoleColor.Green);

                s.BeginReceive(buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), s);
            }
            catch
            {
                Init.logger.ConsoleLog("[GMS] Falha ao se conectar!", ConsoleColor.Red);
                Environment.Exit(0);
                return;
            }
        }
        
        private void ReceiveCallback(IAsyncResult e)
        {
            Socket s = (Socket)e.AsyncState;
            if (s == null) return;
            if (!s.Connected) return;

            try
            {
                int len = s.EndReceive(e);
                if(len > 0)
                {
                    byte[] data = new byte[len];
                    Array.Copy(buffer, data, len);

                    ReceiveManager.Process(data);

                    buffer = new byte[BUFFER_SIZE];

                    s.BeginReceive(buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), s);
                }
                else
                {
                    // disconnect
                }
            }
            catch
            {
                // disconnect
                return;
            }
        }

    }
}
