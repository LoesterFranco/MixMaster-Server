using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Game_Manager_Server.MixMaster_API.Crypto;


namespace Game_Manager_Server.MixMaster_API.Network
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
                    bw.Write(XCRYPT.Encrypt(content, XCRYPT.GetPubKeyIndex(RandomPubKey), XCRYPT.GameManagerServerPrivKey));
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                Send(MyClient, buffer);
            }
            
        }

        public static void SendAprovedAuthentication(ClientManager MyClient)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x07); // packet 
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);
                    bw.Write((byte)0x00);
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendAprovedSession(ClientManager MyClient)
        {
            Send(MyClient, new byte[] {0x02, 0x00, 0x09, 0xa8, 0xc5 });
        }

        public static void SendResponseLoadCharacters(ClientManager MyClient)
        {
            //Console.WriteLine("Sending characters info to: " + MyClient.data.username + ", " + MyClient.data.id_idx);

            // get characters count

            int CharactersCount = Database.gamedata.GetCharactersCount(MyClient.data.id_idx);
            //Console.WriteLine("Characters count: " + CharactersCount);
            List<XHERO> Heroes = Database.gamedata.GetAllHeroesFromID(MyClient.data.id_idx);
            

            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x02); // write packet type
                    //bw.Write((byte)0x00); // ATT NEW CLIENT
                    bw.Write((byte)CharactersCount); // write characters count
                    if(CharactersCount > 0)
                    {
                        for(int i=0; i < Heroes.Count; i++)
                        {
                            /*
                            Console.WriteLine("[" + i + "] Hero_oder: " + Heroes[i].hero_order);
                            Console.WriteLine("[" + i + "] Name: " + Heroes[i].name);
                            Console.WriteLine("[" + i + "] Hero_type: " + Heroes[i].hero_type);
                            Console.WriteLine("[" + i + "] Hero_LV: " + Heroes[i].baselevel);
                            Console.WriteLine("[" + i + "] Avatar_head: " + Heroes[i].avatar_head); */
                            bw.Write((byte)Heroes[i].hero_order);
                            foreach(char c in Heroes[i].name)
                            {
                                bw.Write((byte)c);
                            }
                            bw.Write((byte)0x00);
                            bw.Write((byte)Heroes[i].hero_type);
                            bw.Write((short)Heroes[i].baselevel);
                            bw.Write((short)Heroes[i].avatar_head);
                            bw.Write((short)190); // weapown equiped
                            bw.Write((short)98); // armour equiped
                            if(Heroes[i].status == 1)
                            {
                                bw.Write((byte)0x01);
                                bw.Write((int)Heroes[i].status_time);

                            }
                            else
                            {
                                bw.Write((byte)0x00);
                                bw.Write((int)604800); // unknow
                            }
                            

                            List<XMOB> HeroMobs = Database.gamedata.GetAllMobsEquipedFromID(MyClient.data.id_idx, Heroes[i].hero_order);
                            if (HeroMobs.Count > 0)
                            {
                                bw.Write((byte)HeroMobs.Count);
                                for (int j = 0; j < HeroMobs.Count; j++)
                                {
                                    bw.Write((byte)j);
                                    bw.Write((short)HeroMobs[j].monster_type);

                                }
                            } else { bw.Write((byte)0x00); }
                        }

                        len = (int)bw.BaseStream.Length;
                    }
                    byte[] buffer = ms.GetBuffer();
                    Array.Resize(ref buffer, len);
                    MakePacketAndSend(MyClient, buffer);

                }
            }
        }

        public static void SendResponseGMSInfo(ClientManager MyClient)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x09); // packet type
                    bw.Write((int)MyInfo.GMS_NUM);
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendResponseCreateCharData(ClientManager MyClient)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x03); // packet type
                    bw.Write((byte)0x04); // characters count
                    for(int i =0; i<4; i++)
                    {
                        short Energia = 4;
                        short Agilidade = 4;
                        short Exatidao = 4;
                        short Sorte = 4;
                        short FreePoints = 5;
                        short Unknow = 0;

                        bw.Write((byte)i); // hero index
                        bw.Write((short)Energia);
                        bw.Write((short)Agilidade);
                        bw.Write((short)Exatidao);
                        bw.Write((short)Sorte);
                        bw.Write((short)FreePoints);
                        bw.Write((short)Unknow);

                    }
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendResponseCreateHero(ClientManager MyClient, int type)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x05); // packet 
                    switch (type)
                    {
                        case 0:  // send 0 to create char successfully
                            bw.Write((byte)0x00);
                            break;
                        case 1: // send 1 to name exists
                            bw.Write((byte)0x01);
                            break;
                        case 2:   // send 2 to character generation failure
                            bw.Write((byte)0x02);
                            break;
                        default:
                            bw.Write((byte)0x02);
                            break;

                    }

                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendResponseDeleteChar(ClientManager MyClient, int type)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    bw.Write((byte)0x04); // packet 
                    switch (type)
                    {
                        case 2:   // success
                            bw.Write((byte)0x02);
                            break;
                        default:
                            bw.Write((byte)0x00);
                            break;
                    }

                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendResponseSelectChar(ClientManager MyClient, byte hero_order)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)0x06); // packet type
                    bw.Write((byte)0x00); // response type


                    IPAddress ip = IPAddress.Parse("127.0.0.1");
                    byte[] ip_bytes = ip.GetAddressBytes();

                    bw.Write(ip_bytes); // ZoneServer IP

                    bw.Write((short)20165); // ZoneServer port
                    bw.Write((byte)0x6b); // ZoneServer public cripto

                    Random rd = new Random();
                    int Token = rd.Next(0xFFFFFF);
                    bw.Write((Int32)Token);

                    // Send token to ZoneServer
                    int id_idx = MyClient.data.id_idx;



                    // verify this values

                    // send to zs
                    ZS_Bind.SendToken(id_idx, Token, hero_order);
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

                ClientFunctions.DisconnectClientFromID(MyClient.id);

            }
        }
    }
}
