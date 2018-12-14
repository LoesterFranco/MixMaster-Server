using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Game_Manager_Server.MixMaster_API.Crypto;

namespace Game_Manager_Server.MixMaster_API.Network
{
    public class LGS_Conn
    {

        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 4096;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        public static Socket _LgsConn;
        private const int BUFFER_SIZE = 4096;
        private static byte[] Buffer = new byte[BUFFER_SIZE];


        public static bool Start()
        {
            try
            {
                _LgsConn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _LgsConn.BeginConnect(MyInfo.LS_IP, MyInfo.LS_PORT, new AsyncCallback(LGSConnect), _LgsConn);
                Receive(_LgsConn);
                return true;
            }
            catch
            {
                return false;

            }

        }

        private static void LGSConnect(IAsyncResult ar)
        {
            _LgsConn = (Socket)ar.AsyncState;
            try
            {
                _LgsConn.EndConnect(ar);
                InitializeHandShakeLGS();
                

            }
            catch
            {
                Console.WriteLine("FAILED LGS CONNECTION ERROR");
                Environment.Exit(0);
                return;
            }
        }



        private static void Receive(Socket client)
        {
            try
            {  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(LGS_Recvc), state);
            }
            catch
            {
                Console.WriteLine("[LGS] Connection failed!");
            }
        }


        private static void LGS_Recvc(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            try
            {
                int ReceivedBytes = client.EndReceive(ar);
                if(ReceivedBytes > 0)
                {
                    byte[] data = new byte[ReceivedBytes];
                    Array.Copy(state.buffer, data, ReceivedBytes);
                    ProcessLGSReceiveData(data);

                    state.buffer = new byte[StateObject.BufferSize];
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(LGS_Recvc), state);
                }
                
            }
            catch
            {
                return;
            }
        }


        public static void SendTOLGS(Socket s, byte[] data)
        {
            try
            {
                s.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendTOLGSCallback), s);
            }
            catch
            {
                return;
            }
        }

        private static void SendTOLGSCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;
                s.EndReceive(ar);
            }
            catch
            {
                return;
            }
        }

        private static void MakePacketAndSend(Socket s, byte[] content)
        {
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

                SendTOLGS(s, buffer);
            }

        }

        private static void InitializeHandShakeLGS()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    string HANDSHAKE_PASSWORD = MyInfo.LGS_PASSWORD;
                    string GMS_SERVERNAME = MyInfo.SERVER_NAME;
                    string GMS_IP = MyInfo.MY_IP.ToString();
                    short GMS_PORT = MyInfo.MY_PORT;
/*
                    Console.WriteLine("HANDSHAKE_PASSWORD: " + HANDSHAKE_PASSWORD);
                    Console.WriteLine("GMS_SERVERNAME: " + GMS_SERVERNAME);
                    Console.WriteLine("GMS_IP: " + GMS_IP);
                    Console.WriteLine("GMS_PORT: " + GMS_PORT);*/

                    bw.Write((byte)0x01);
                    bw.Write((byte)0x00);
                    foreach(char c in HANDSHAKE_PASSWORD)
                    {
                        bw.Write((byte)c);
                    }
                    bw.Write((byte)0x00);
                    foreach(char c in GMS_SERVERNAME)
                    {
                        bw.Write((byte)c);
                    }
                    bw.Write((byte)0x00);
                    foreach(char c in GMS_IP)
                    {
                        bw.Write((byte)c);
                    }
                    bw.Write((byte)0x00);
                    bw.Write((short)GMS_PORT);


                    len = (int)bw.BaseStream.Length;
                }

                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(_LgsConn, buffer);
            }

        }

        private static void ProcessLGSReceiveData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short DataLen = br.ReadInt16();
                    byte packetType = br.ReadByte();
                    byte[] Data = br.ReadBytes(DataLen);

                    ParsingLGSDataReceived(packetType, Data);

                    br.Close();
                }
                ms.Close();
            }
        }

        private static void ParsingLGSDataReceived(byte packetType, byte[] packetData)
        {
            switch (packetType)
            {
                case 1:
                    ReadAuthLGS(packetData);
                    break;
                default:
                    Console.WriteLine("[LGS] Packet type not found");
                    break;
            }
        }


        private static void ReadAuthLGS(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    int id_idx = br.ReadInt32(); // id_idx
                    char c;
                    string username = "";
                    while((c = br.ReadChar()) != (char)0x00)
                    {
                        username += c;
                    }

                    int LGS_Token = br.ReadInt32();

                    //Console.WriteLine("[W] Username: " + username + ", id_idx: " + id_idx + ", Token: " + LGS_Token);

                    SessionLGSToken Temp = new SessionLGSToken();
                    Temp.id_idx = id_idx;
                    Temp.LGS_Token = LGS_Token;
                    Temp.username = username;


                    init.SessionLGS.Add(Temp);

                }
            }
            
        }
    }
}
