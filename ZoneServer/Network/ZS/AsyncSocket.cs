using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace ZoneServer.Network.ZS
{
    public class AsyncSocket
    {
        public static Socket ZS_Listener;

        public static bool Start()
        {
            try
            {
                ZS_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ZS_Listener.Bind(new IPEndPoint(MyInfo.ZC_BIND_IP, MyInfo.ZC_BIND_PORT));
                Thread SocketThread = new Thread(new ThreadStart(ZS_Listen));
                SocketThread.Start();
                return true;
            }
            catch
            {
                LogManager.CLogManager.WriteConsoleLog("[-] Fatal error: Socket can't initialize", ConsoleColor.Red);
                return false;
            }
        }

        private static void ZS_Listen()
        {
            try
            {
                ZS_Listener.Listen(MyInfo.ZC_MAXCLIENT);
                ZS_Listener.BeginAccept(new AsyncCallback(ZS_Accept), ZS_Listener);
            }
            catch
            {
                return;
            }
        }

        private static void ZS_Accept(IAsyncResult ar)
        {
            Socket ZS_Listener = (Socket)ar.AsyncState;
            try
            {
                Socket client = ZS_Listener.EndAccept(ar);
                if (client != null)
                {
                    int ClientID = client.GetHashCode();
                    if (!XCLIENT.IsClientIdExist(ClientID))
                    {
                        Client MyClient = new Client(ClientID, client);
                        XCLIENT.Clients.Add(MyClient);

                        int ClientIndex = XCLIENT.GetClientIndexInList(ClientID);
                        if (ClientIndex != -1)
                        {
                            LogManager.CLogManager.WriteConsoleLog("[ZS_ACCEPT] Client Connected!", ConsoleColor.Green);
                            XCLIENT.Clients[ClientIndex].socket.BeginReceive(XCLIENT.Clients[ClientIndex].buffer, 0, Client.BufferSize, SocketFlags.None, new AsyncCallback(ZS_Receive), XCLIENT.Clients[ClientIndex]);
                            SendData.SendHello(MyClient);
                        }
                    }
                    else
                    {
                        LogManager.CLogManager.WriteConsoleLog("[ZS_ACCEPT] Client add in list Error", ConsoleColor.Red);
                    }
                }
            }
            catch
            {
                LogManager.CLogManager.WriteConsoleLog("[ZS_ACCEPT] Connection Client Error", ConsoleColor.Red);
                return;
            }
            finally
            {
                ZS_Listener.BeginAccept(new AsyncCallback(ZS_Accept), ZS_Listener);
            }
        }


        public const int FIONREAD = 0x4004667F;
        public static int getPendingByteCount(Socket s)
        {
            try
            {
                byte[] outValue = BitConverter.GetBytes(0);
                s.IOControl(FIONREAD, null, outValue);
                int bytesAvailable = BitConverter.ToInt32(outValue, 0);
                return bytesAvailable;
            }
            catch
            {
                return 0;
            }
        }

        private static void ZS_Receive(IAsyncResult ar)
        {
            Client MyClient = (Client)ar.AsyncState;
            Socket client = MyClient.socket;
            if (client == null) { return; }
            if (!client.Connected) { return; }
            int BufferSize = getPendingByteCount(client);
            if (BufferSize > 4000) { return; } // limit maximo bytes receive
            byte[] Data;
            try
            {
                int BytesReceive = client.EndReceive(ar);
                if(BytesReceive > 0)
                {
                    Data = new byte[BytesReceive];
                    Array.Copy(MyClient.buffer, Data, BytesReceive);
                    ReceiveData.Handle_Client_Packet(MyClient, Data);
                    MyClient.buffer = new byte[Client.BufferSize];
                }
                else
                {
                    XCLIENT.DisconnectClientFromID(MyClient.ID);
                    XCLIENT.RemoveClientFromList(MyClient);
                }
                client.BeginReceive(MyClient.buffer, 0, Client.BufferSize, SocketFlags.None, new AsyncCallback(ZS_Receive), MyClient);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    XCLIENT.DisconnectClientFromID(MyClient.ID);
                    XCLIENT.RemoveClientFromList(MyClient);
                    return;
                }
            }
            catch
            {
                XCLIENT.DisconnectClientFromID(MyClient.ID);
                XCLIENT.RemoveClientFromList(MyClient);
                return;
            }
        }


    }
}
