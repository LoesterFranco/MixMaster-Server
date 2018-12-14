using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Login_Server.MixMaster_API.Network
{
    public class GMS_bind
    {
        public class GMS_Data
        {
            public byte gms_servers_count;
            public byte gms_serverip_count;
            public string server_name;
            public string gms_serverip;
            public short gms_port;
            public short players_online;
        }

        public class GMS_Manager
        {
            public int id;
            public bool authenticated;
            public Socket _socket;
            public const int BufferSize = 4096;
            public byte[] buffer = new byte[BufferSize];
            public GMS_Data data;
            public GMS_Manager(int id, Socket Client)
            {
                this.id = id;
                this._socket = Client;
            }
        }






        public static Socket _Listener;
        public static List<GMS_Manager> GMS_Clients = new List<GMS_Manager>();

        public static bool Start()
        {
            try
            {
                _Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _Listener.Bind(new IPEndPoint(MyInfo.GMS_SERVER_IP, MyInfo.GMS_PORT));
                Thread SocketThread = new Thread(new ThreadStart(InitializeGMSBind));
                SocketThread.Start();
                //Console.WriteLine("[GMS Socket] IP: " + MyInfo.GMS_SERVER_IP);
                //Console.WriteLine("[GMS Socket] Port: " + MyInfo.GMS_PORT);
                return true;
            }
            catch
            {
                Console.WriteLine("[-] Fatal error: Socket can't initialize");
                Console.ReadLine();
                return false;
            }
        }

        private static void InitializeGMSBind()
        {
            try
            {
                _Listener.Listen(MyInfo.MaxConnections);
                _Listener.BeginAccept(new AsyncCallback(GMSBindAccept), _Listener);
            }
            catch
            {
                return;
            }
        }

        private static int GetClientIndex(int clientID)
        {
            for (int i = 0; i < GMS_Clients.Count; i++)
            {
                if (GMS_Clients[i].id == clientID)
                {
                    return i;
                }
            }
            return -1;
        }

        private static void DisconnectClientFromID(int ClientID)
        {
            for (int i = 0; i < GMS_Clients.Count; i++)
            {
                if (GMS_Clients[i].id == ClientID)
                {
                    GMS_Clients[i]._socket.Close();
                    GMS_Clients[i]._socket = null;
                    GMS_Clients.RemoveAt(i);
                }
            }
        }


        private static void GMSBindAccept(IAsyncResult ar)
        {
            Socket _listen = (Socket)ar.AsyncState;
            try
            {
                Socket client = _listen.EndAccept(ar);
               
                if (client != null)
                {
                    int ClientID = client.GetHashCode();
                    Console.WriteLine("[W] GameManagerServer Connected");
                    GMS_Manager MyGMS = new GMS_Manager(ClientID, client);
                    GMS_Clients.Add(MyGMS);
                    int GMSClientIndex = GetClientIndex(ClientID);
                    GMS_Clients[GMSClientIndex]._socket.BeginReceive(GMS_Clients[GMSClientIndex].buffer, 0, GMS_Manager.BufferSize, SocketFlags.None, new AsyncCallback(GMSBind_receive), GMS_Clients[GMSClientIndex]);

                }
            }
            catch
            {
                return;
            }
            finally
            {
                _listen.BeginAccept(new AsyncCallback(GMSBindAccept), _listen);
            }
        }
        

        private static void GMSBind_receive(IAsyncResult ar)
        {
            byte[] Data;
            GMS_Manager My_GMS = (GMS_Manager)ar.AsyncState;
            Socket client = My_GMS._socket;


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
                    Array.Copy(My_GMS.buffer, Data, BytesReceive);
                    // process byte
                    
                    HandleGMSReceive(My_GMS, Data);
                    My_GMS.buffer = new byte[GMS_Manager.BufferSize];
                    

                }
                else
                {
                    DisconnectClientFromID(My_GMS.id);
                    //disconnect
                }
                client.BeginReceive(My_GMS.buffer, 0, GMS_Manager.BufferSize, SocketFlags.None, new AsyncCallback(GMSBind_receive), My_GMS);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    DisconnectClientFromID(My_GMS.id);
                    //disconnect
                    return;
                }
            }
            catch
            {
                DisconnectClientFromID(My_GMS.id);
                //disconnect
                return;
            }
        }


        private static void HandleGMSReceive(GMS_Manager MyGMS, byte[] data)
        {
            // struct packet gms
            // 2 bytes - tamanho
            // 1 byte - packet type
            // data
            //Console.WriteLine(data.Length);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short DataLen = br.ReadInt16();
                    byte packetType = br.ReadByte();
                    byte[] Data = br.ReadBytes(DataLen);

                    ParsingGMSDataReceived(packetType, Data);

                    br.Close();
                }
                ms.Close();
            }
        }

        public static void SendToGMS(byte[] data)
        {
            try
            {
                //s.BeginSend(data, 0, GMS_Manager.BufferSize, SocketFlags.None, new AsyncCallback(SendToGMSCallback), s);
                GMS_Clients[0]._socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendToGMSCallback), GMS_Clients[0]._socket);
            }
            catch
            {
                return;
            }
        }

        private static void SendToGMSCallback(IAsyncResult ar)
        {
            try
            {
                Socket _socket = (Socket)ar.AsyncState;
                _socket.EndSend(ar);
            }
            catch
            {
                return;
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

                SendToGMS(buffer);
                
            }

        }



      

        private static void ParsingGMSDataReceived(byte packetType, byte[] packetData)
        {
            switch(packetType)
            {
                case 1: // handshake login server ~ GameManagerServer
                    //Console.WriteLine("[GMS] Receive handshake info");
                    ParseHandshakeReceived(packetData);
                    break;
                case 2: // Att info in LoginServer
                    break;
                case 3: // confirm connect server
                    break;
                default:
                    //Console.WriteLine("[GMS] Packet type not found!");
                    // packet type not found
                    break;
            }
        }

        private static void ParseHandshakeReceived(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.ReadByte(); // packet type
                    //br.ReadByte(); // null byte

                    string HANDSHAKE_PASSWORD = "";
                    string GMS_SERVERNAME = "";
                    string GMS_IP = "";
                    short GMS_PORT;

                    char c;
                    while((c = br.ReadChar()) != (char)0x00)
                    {
                        HANDSHAKE_PASSWORD += c;
                    }
                    //Console.WriteLine("HandShake Password: " + HANDSHAKE_PASSWORD);

                    while ((c = br.ReadChar()) != (char)0x00)
                    {
                        GMS_SERVERNAME += c;
                    }
                    //Console.WriteLine("GMS_SERVERNAME: " + GMS_SERVERNAME);

                    while ((c = br.ReadChar()) != (char)0x00)
                    {
                        GMS_IP += c;
                    }
                   // Console.WriteLine("GMS_IP: " + GMS_IP);


                    GMS_PORT = br.ReadInt16();
                    //Console.WriteLine("GMS_PORT: " + GMS_PORT);


                    // verify password connection, maybe not utilize

                    MyInfo.gms_servers_count = 1;
                    MyInfo.gms_serverip_count = 1;
                    MyInfo.gms_serverip = GMS_IP;
                    MyInfo.gms_port = GMS_PORT;
                    MyInfo.server_name = GMS_SERVERNAME;
                    MyInfo.players_online = 0;


                    MyInfo.GMS_HANDSHAKE_SUCCESS = true;

                }
            }
        }

        public static void SendToGMSAuth(string username, int id_idx, int token)
        {
            int len = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)1); // packet type
                    bw.Write(id_idx);
                    foreach(char c in username)
                    {
                        bw.Write((char)c);
                    }
                    bw.Write((byte)0x00);
                    //bw.Write(username);
                    bw.Write(token);
                    len = (int)bw.BaseStream.Length;
                }

                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);
                MakePacketAndSend(content);
            }
        }


    }
}
