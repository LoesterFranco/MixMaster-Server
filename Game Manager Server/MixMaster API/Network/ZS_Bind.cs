using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Game_Manager_Server.MixMaster_API.Network
{
    public class ZS_Bind
    {
        
        public class ZS_Manager
        {
            public int id;
            public bool authenticated;
            public Socket _socket;
            public const int BufferSize = 4096;
            public byte[] buffer = new byte[BufferSize];
            public ZS_Manager(int id, Socket Client)
            {
                this.id = id;
                this._socket = Client;
            }
        }


        public static Socket _Listener;
        public static List<ZS_Manager> ZS_Clients = new List<ZS_Manager>();

        public static bool Start()
        {
            try
            {
                _Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _Listener.Bind(new IPEndPoint(MyInfo.ZS_BIND_IP, MyInfo.ZS_BIND_PORT));
                Thread SocketThread = new Thread(new ThreadStart(InitializeZSBind));
                SocketThread.Start();
                Console.WriteLine("[ZS Socket] IP: " + MyInfo.ZS_BIND_IP);
                Console.WriteLine("[ZS Socket] Port: " + MyInfo.ZS_BIND_PORT);
                return true;
            }
            catch
            {
                Console.WriteLine("[-] Fatal error: Socket can't initialize");
                Console.ReadLine();
                return false;
            }
        }

        private static void InitializeZSBind()
        {
            try
            {
                _Listener.Listen(MyInfo.MAX_CONNECTIONS);
                _Listener.BeginAccept(new AsyncCallback(ZSBindAccept), _Listener);
            }
            catch
            {
                return;
            }
        }

        private static int GetClientIndex(int clientID)
        {
            for (int i = 0; i < ZS_Clients.Count; i++)
            {
                if (ZS_Clients[i].id == clientID)
                {
                    return i;
                }
            }
            return -1;
        }

        private static void DisconnectClientFromID(int ClientID)
        {
            for (int i = 0; i < ZS_Clients.Count; i++)
            {
                if (ZS_Clients[i].id == ClientID)
                {
                    ZS_Clients[i]._socket.Close();
                    ZS_Clients[i]._socket = null;
                    ZS_Clients.RemoveAt(i);
                }
            }
        }


        private static void ZSBindAccept(IAsyncResult ar)
        {
            Socket _listen = (Socket)ar.AsyncState;
            try
            {
                Socket client = _listen.EndAccept(ar);
                if (client != null)
                {
                    int ClientID = client.GetHashCode();
                    Console.WriteLine("[W] ZoneServer Connected");
                    ZS_Manager MyZS = new ZS_Manager(ClientID, client);
                    ZS_Clients.Add(MyZS);

                    int GMSClientIndex = GetClientIndex(ClientID);
                    ZS_Clients[GMSClientIndex]._socket.BeginReceive(ZS_Clients[GMSClientIndex].buffer, 0, ZS_Manager.BufferSize, SocketFlags.None, new AsyncCallback(ZSBind_receive), ZS_Clients[GMSClientIndex]);

                }
            }
            catch
            {
                return;
            }
            finally
            {
                _listen.BeginAccept(new AsyncCallback(ZSBindAccept), _listen);
            }
        }
        

        private static void ZSBind_receive(IAsyncResult ar)
        {
            byte[] Data;
            ZS_Manager My_ZS = (ZS_Manager)ar.AsyncState;
            Socket client = My_ZS._socket;


            if (client == null) { return; }
            if (!client.Connected) { return; }
            int BufferSize = AsyncSocket.getPendingByteCount(client);
            if (BufferSize > 1000) { return; }

            try
            {
                int BytesReceive = client.EndReceive(ar);
                if (BytesReceive > 0)
                {
                    Data = new byte[BytesReceive];
                    Array.Copy(My_ZS.buffer, Data, BytesReceive);
                    HandleZSReceive(My_ZS, Data);
                    My_ZS.buffer = new byte[ZS_Manager.BufferSize];
                }
                else
                {
                    DisconnectClientFromID(My_ZS.id);
                }
                client.BeginReceive(My_ZS.buffer, 0, ZS_Manager.BufferSize, SocketFlags.None, new AsyncCallback(ZSBind_receive), My_ZS);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    DisconnectClientFromID(My_ZS.id);
                    //disconnect
                    return;
                }
            }
            catch
            {
                DisconnectClientFromID(My_ZS.id);
                //disconnect
                return;
            }
        }


        private static void HandleZSReceive(ZS_Manager My_ZS, byte[] data)
        {
            // struct packet zs
            // 2 bytes - tamanho
            // 1 byte - packet type
            // data

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short DataLen = br.ReadInt16();
                    byte packetType = br.ReadByte();
                    byte[] Data = br.ReadBytes(DataLen);

                    ParsingZSDataReceived(packetType, Data, My_ZS);

                    br.Close();
                }
                ms.Close();
            }
        }


        public static void SendToZoneServer(ZS_Manager ZS, byte[] data)
        {
            try
            {
                ZS._socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendZSCallback), ZS._socket);
            }
            catch
            {
                return;
            }
        }

        private static void SendZSCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch
            {
                return;
            }
        }

        private static void ParsingZSDataReceived(byte packetType, byte[] packetData, ZS_Manager My_ZS)
        {
            switch(packetType)
            {
                default:
                    Console.WriteLine("[ZS] Packet type not found!");
                    
                    break;
            }
        }




        private static void MakePacketAndSend(byte[] content)
        {
            //Console.WriteLine("Sending content to GMS!");
            //Console.WriteLine("GMS Count: " + GMS_Clients.Count);
            short ContentLenght = (short)content.Length;
            //Console.WriteLine("Sending: " + ContentLenght + " | " + RandomPubKey);

            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((short)ContentLenght);
                    bw.Write(content);
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                SendToZoneServer(ZS_Clients[0], buffer);

            }

        }

        public static void SendToken(int id_idx, int token, byte hero_order)
        {
            int len = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)2); // packet type
                    bw.Write(id_idx);
                    bw.Write(token);
                    bw.Write((byte)hero_order);
                    len = (int)bw.BaseStream.Length;
                }

                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);
                MakePacketAndSend(content);
            }
        }


    }
}
