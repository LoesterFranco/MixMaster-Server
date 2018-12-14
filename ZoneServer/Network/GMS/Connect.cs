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
    public class Connect
    {
        public class Conn
        {
            public Socket socket = null;
            public const int BufferSize = 4096; 
            public byte[] buffer = new byte[BufferSize];
        }

        public static bool Start()
        {
            try
            {
                Conn conn = new Conn();
                conn.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                conn.socket.BeginConnect(MyInfo.GMS_IP, MyInfo.GMS_PORT, new AsyncCallback(GMSConnect), conn);
                Receive(conn);
                return true;
            }
            catch
            {
                return false;

            }

        }

        private static void GMSConnect(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            try
            {
                conn.socket.EndConnect(ar);
               // InitializeHandShakeLGS();
            }
            catch
            {
                LogManager.CLogManager.WriteConsoleLog("[GMS] Connection failed ...", ConsoleColor.Red);
                Environment.Exit(0);
                return;
            }
        }

        private static void Receive(Conn conn)
        {
            try
            {
                conn.socket.BeginReceive(conn.buffer, 0, Conn.BufferSize, 0,
                    new AsyncCallback(GMS_Recv), conn);
            }
            catch
            {
                LogManager.CLogManager.WriteConsoleLog("[GMS] Connection failed ...", ConsoleColor.Red);
            }
        }

        private static void GMS_Recv(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            Socket client = conn.socket;
            try
            {
                int ReceivedBytes = client.EndReceive(ar);
                if (ReceivedBytes > 0)
                {
                    byte[] data = new byte[ReceivedBytes];
                    Array.Copy(conn.buffer, data, ReceivedBytes);
                    ReceiveData.ProcessGMSReceiveData(data);

                    conn.buffer = new byte[Conn.BufferSize];
                    client.BeginReceive(conn.buffer, 0, Conn.BufferSize, SocketFlags.None, new AsyncCallback(GMS_Recv), conn);
                }
            }
            catch
            {
                return;
            }
        }

    }
}
