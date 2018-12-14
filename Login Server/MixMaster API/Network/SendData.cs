using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Login_Server.MixMaster_API.Crypto;
using Login_Server.MixMaster_API.Network;


namespace Login_Server.MixMaster_API.Network
{
    public class SendData
    {
        public static void Send(ClientManager client, byte[] data)
        {
            try
            {
                Socket _sock = client._socket;
                if(_sock.Connected)
                {
                    _sock.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), _sock);
                }
                
            }
            catch
            {
                return;
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket _sock = (Socket)ar.AsyncState;
                _sock.EndSend(ar);
            }
            catch
            {
                return;
            }
        }

        private static void MakePacketAndSend(ClientManager MyClient, byte[] content)
        {
            short ContentLenght = (short)content.Length;
            byte RandomPubKey = XCRYPT.AddRandPubKey();
 

            //Console.WriteLine("Sending: " + ContentLenght + " | " + RandomPubKey);

            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((short)ContentLenght);
                    bw.Write((byte)RandomPubKey);
                    bw.Write(XCRYPT.Encrypt(content, XCRYPT.GetPubKeyIndex(RandomPubKey), XCRYPT.LoginServerPrivkey));
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                Send(MyClient, buffer);
            }
            
        }

        public static void SendVersionResponse(ClientManager MyClient, int type)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    if (type == 1) // version ok
                    {
                        bw.Write((byte)0x0B);
                    }

                    else if (type == 2) // version is very low
                    {
                        bw.Write((byte)0x0C);
                    }
                    else if (type == 3) // send version with ftpadr
                    {
                        bw.Write((byte)0x0C);
                        bw.Write((byte)0x02);
                        bw.Write((byte)0x14);
                        bw.Write((byte)0x77);
                        bw.Write((byte)0x40);
                        bw.Write((byte)0x01);
                        bw.Write(MyInfo.FTPAdr);
                    }
                    len = (int)bw.BaseStream.Length;

                }
                stream.Flush();
                byte[] content = stream.GetBuffer();
                Array.Resize(ref content, len);
                //Console.WriteLine("Sending version response!");
                MakePacketAndSend(MyClient, content);

            }
        }


        public static void SendLoginResponse(ClientManager MyClient, Credentials MyCredential)
        {
            MyClient.data.username = MyCredential.Username;
            MyClient.data.password = MyCredential.Password;
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    // check login
                    // int response = Database.Member.CheckLogin(MyCredential);
                    // string blockstatus = Database.Member.CheckBlock(MyCredential.Username)
                    int respose = Database.Member.CheckLogin(MyCredential);
                    MyClient.data.id_idx = Database.Member.UpdateIDX(MyClient.data.username);
                    
                    switch(respose)
                    {
                        case 1: // Password Incorrect
                            bw.Write((byte)24);
                            break;

                        case 2: // Username and Passwords Incorrect
                            bw.Write((byte)23);
                            break;

                        case 3: // login success
                            if (MyInfo.GMS_HANDSHAKE_SUCCESS)
                            {
                                bw.Write((byte)0x16);

                                bw.Write((byte)MyInfo.gms_servers_count);
                                bw.Write((byte)0x00);

                                if(MyInfo.server_name.Length > 16)
                                {
                                    return;
                                }

                                byte[] server_name_bytes = new byte[16];
                                for(int i=0; i< MyInfo.server_name.Length; i++)
                                {
                                    server_name_bytes[i] = (byte)MyInfo.server_name[i];
                                }

                                bw.Write(server_name_bytes);
                                bw.Write((byte)0x00);
                                bw.Write((short)MyInfo.players_online);
                                bw.Write((byte)MyInfo.gms_serverip_count);

                                byte[] server_ip_bytes = new byte[15];
                                for(int i=0; i < MyInfo.gms_serverip.Length; i++ )
                                {
                                    server_ip_bytes[i] = (byte)MyInfo.gms_serverip[i];
                                }

                                bw.Write(server_ip_bytes);

                                bw.Write((byte)0x00);
                                bw.Write((short)MyInfo.gms_port);
                            }
                            else
                            {
                                bw.Write((byte)0x16);
                            }
                            //Console.WriteLine(MyCredential.Username + " is logged!");
                            break;

                        case 4: // account blocked
                            //Console.WriteLine(MyCredential.Username + " has banned");
                            break;
                    }

                    len = (int)bw.BaseStream.Length;
                }
                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);

                MakePacketAndSend(MyClient, content);
            }
        }


        // used by mixer.exe
        public static void SendLoginResponse2(ClientManager MyClient, Credentials MyCredential)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    // check login
                    // int response = Database.Member.CheckLogin(MyCredential);
                    // string blockstatus = Database.Member.CheckBlock(MyCredential.Username)
                    int respose = Database.Member.CheckLogin(MyCredential);

                    switch (respose)
                    {
                        case 1: // Password Incorrect
                            bw.Write((byte)24);
                            break;

                        case 2: // Username and Passwords Incorrect
                            bw.Write((byte)23);
                            break;

                        case 3: // login success
                            bw.Write((byte)22);
                            bw.Write((byte)150);
                            bw.Write((byte)1);
                            //Console.WriteLine(MyCredential.Username + " is logged!");
                            break;

                        case 4: // account blocked
                            //Console.WriteLine(MyCredential.Username + " has banned");
                            break;
                    }

                    len = (int)bw.BaseStream.Length;
                }
                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);

                MakePacketAndSend(MyClient, content);
            }
        }
        // used by mixer.exe
        public static void SendLoginResponse3(ClientManager MyClient, Credentials MyCredential)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    // check login
                    // int response = Database.Member.CheckLogin(MyCredential);
                    // string blockstatus = Database.Member.CheckBlock(MyCredential.Username)
                    int respose = Database.Member.CheckLogin(MyCredential);

                    switch (respose)
                    {
                        case 1: // Password Incorrect
                            bw.Write((byte)24);
                            break;

                        case 2: // Username and Passwords Incorrect
                            bw.Write((byte)23);
                            break;

                        case 3: // login success
                            bw.Write((byte)22);
                            bw.Write((byte)154);
                            bw.Write((byte)1);
                            //Console.WriteLine(MyCredential.Username + " is logged!");
                            break;

                        case 4: // account blocked
                            //Console.WriteLine(MyCredential.Username + " has banned");
                            break;
                    }

                    len = (int)bw.BaseStream.Length;
                }
                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);

                MakePacketAndSend(MyClient, content);
            }
        }

        public static void SendServerList(ClientManager MyClient)
        {
            int len = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    if(MyInfo.GMS_HANDSHAKE_SUCCESS)
                    {
                        bw.Write((byte)0x16);
                        
                        bw.Write((byte)MyInfo.gms_servers_count);
                        bw.Write((byte)0x00);

                        foreach (char c in MyInfo.server_name)
                        {
                            bw.Write((byte)c);
                        }
                        bw.Write((byte)0x00);
                        bw.Write((short)0x00);
                        bw.Write((short)0x00);
                        bw.Write((short)MyInfo.players_online);
                        bw.Write((byte)MyInfo.gms_serverip_count);
                        foreach (char c in MyInfo.gms_serverip)
                        {
                            bw.Write((byte)c);
                        }
                        bw.Write((byte)0x00);
                        bw.Write((short)0x00);
                        bw.Write((short)MyInfo.gms_port);
                    }
                    else
                    {
                        bw.Write((byte)0x16);
                    }
                    
                    len = (int)bw.BaseStream.Length;
                }

                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);
                MakePacketAndSend(MyClient, content);
            }
        }


        public static void SendResponseEnterServer(ClientManager MyClient)
        {
            int len = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)28); // packet type
                    bw.Write((Int32)MyClient.data.id_idx);
                    Random rand = new Random();
                    MyClient.data.GMS_TOKEN = rand.Next((int)0xffffff);
                    bw.Write((Int32)MyClient.data.GMS_TOKEN);

                    GMS_bind.SendToGMSAuth(MyClient.data.username, MyClient.data.id_idx, MyClient.data.GMS_TOKEN);

                    len = (int)bw.BaseStream.Length;
                }

                byte[] content = ms.GetBuffer();
                Array.Resize(ref content, len);
                MakePacketAndSend(MyClient, content);
                ClientFunctions.DisconnectClientFromID(MyClient.id);
            }
        }

    }
}
