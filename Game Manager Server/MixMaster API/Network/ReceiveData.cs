using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Game_Manager_Server.MixMaster_API;
using Game_Manager_Server.MixMaster_API.Crypto;

namespace Game_Manager_Server.MixMaster_API.Network
{
    public class ReceiveData
    {
        public static void ParsePacket(ClientManager MyClient, byte[] data)
        {
            if (MyClient == null) { return; }
            if (MyClient._socket == null) { return; }
            if (!MyClient._socket.Connected) { return; }

            byte[] PacketDecrypted = PacketFunctions.GetPacketDataDecrypted(data);

            using (MemoryStream ms = new MemoryStream(PacketDecrypted))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int id_idx = br.ReadInt32();
                    if(!ClientFunctions.SearchUserIDInSession(id_idx))
                    {
                        Console.WriteLine("Client not found in list");
                        return;
                    }


                    br.ReadByte();
                    byte PacketType = br.ReadByte();

                    if (MyClient.data.Authenticated)
                    {
                        switch (PacketType)
                        {
                            case 2: // load characters data
                                //Console.WriteLine("Receive load character!");
                                SendData.SendResponseLoadCharacters(MyClient);
                                SendData.SendResponseGMSInfo(MyClient);
                                return;
                                break;
                            case 3: // send create character prev data
                                //Console.WriteLine("Receive get create char data!");
                                SendData.SendResponseCreateCharData(MyClient);
                                return;
                                break;
                            case 4: // delete character
                                //Console.WriteLine("Receive delete hero request!");
                                ProcessDeleteHeroReceive(MyClient, PacketDecrypted);
                                return;

                                break;
                            case 5: // create hero
                                //Console.WriteLine("Receive create hero!");
                                ProcessCreateHeroReceive(MyClient, PacketDecrypted);
                                return;
                                break;
                            case 6: // select hero
                                Console.WriteLine("[HeroSelect] Request hero select.");
                                ProcessHeroSelected(MyClient, PacketDecrypted);
                                break;
                            case 10: // guild mark list
                                break;
                            case 32: // change heroname
                                break;
                        }
                    }
           
                    float exe_version = br.ReadSingle();
                    if(exe_version != MyInfo.EXE_VERSION)
                    {
                        //Console.WriteLine("Client use exe_version old version");
                        // disconnect by using client old_version
                        return;
                    }

                    int id_idx2 = br.ReadInt32();
                    int LGS_TOKEN = br.ReadInt32();
                    byte[] username_bytes = br.ReadBytes(32);
                    string username = PacketFunctions.ExtractStringFromBytes(username_bytes);

                    if(!ClientFunctions.TokenIsValidInSession(id_idx2, LGS_TOKEN, username))
                    {
                        //Console.WriteLine("Incorrect token!");
                        // disconnect by using a incorrect token
                        return;
                    }

                    MyClient.data.id_idx = id_idx2; // copy id_idx valid from packet
                    MyClient.data.username = username; // copy username valid from packet
                    MyClient.data.Authenticated = true; // confirm client authenticated aprove
                    SendData.SendAprovedAuthentication(MyClient);
                    SendData.SendAprovedSession(MyClient);
                    //Console.WriteLine("[W] User auth aproved! " + username + ":" + id_idx2);
                    
                }
            }
        }

        private static void ProcessPacket(ClientManager MyClient, ReadPacket CPacket)
        {
            Header PacketHead = CPacket.GetHeader();
            byte[] PacketBody = CPacket.GetBody();
            byte PacketType = CPacket.GetPacketType();

            //Console.WriteLine("Receive Packet len: " + PacketHead.len);
            //Console.WriteLine("Packet Type: " + PacketType);


            switch(PacketType)
            {
               
            }
        }

        private static void ProcessDeleteHeroReceive(ClientManager MyClient, byte[] PacketData)
        {
            using (MemoryStream ms = new MemoryStream(PacketData))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    br.ReadInt32(); // id_idx
                    br.ReadByte(); // byte null
                    br.ReadByte(); // packet type

                    byte hero_order = br.ReadByte();

                    if(Database.gamedata.PutDeleteCharDate(MyClient.data.id_idx, hero_order))
                    {
                        SendData.SendResponseDeleteChar(MyClient, 2); // send success
                    }
                }
            }
        }

        private static void ProcessCreateHeroReceive(ClientManager MyClient, byte[] PacketData)
        {
            using (MemoryStream ms = new MemoryStream(PacketData))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    br.ReadInt32(); // id_idx
                    br.ReadByte(); // byte null
                    br.ReadByte(); // packet type

                    byte Hero_Type = br.ReadByte();
                    short Energia = br.ReadInt16();
                    short Agilidade = br.ReadInt16();
                    short Exatidao = br.ReadInt16();
                    short Sorte = br.ReadInt16();
                    short unknow = br.ReadInt16();
                    short unknow2 = br.ReadInt16();
                    byte hero_order = br.ReadByte();
                    string heroname = "";
                    char c;
                    while((c = br.ReadChar()) != (char)0x00)
                    {
                        heroname += c;
                    }

                    // verificar se os tributos recebidos estão dentro do limite suportado
                    // verificar o tamanho do nome do personagem, se passar de 12 descarta
                    XHERO MyHero = new XHERO();
                    MyHero.avatar_head = (int)unknow2;
                    MyHero.hero_type = (int)Hero_Type;
                    MyHero.name = heroname;
                    MyHero.hero_order = hero_order;
                    MyHero.Agilidade = Agilidade;
                    MyHero.Energia = Energia;
                    MyHero.Exatidao = Exatidao;
                    MyHero.Sorte = Sorte;
                    
                    if (!Database.gamedata.CharacterNameExists(heroname))
                    {
                        if (Database.gamedata.CreateCharacter(MyClient, MyHero))
                        {
                            SendData.SendResponseCreateHero(MyClient, 0); // create success
                        }
                        else
                        {
                            SendData.SendResponseCreateHero(MyClient, 2); // create failed
                        }
                    }
                    else
                    {
                        SendData.SendResponseCreateHero(MyClient, 1); // username exist
                    }

                    
                }
            }
        }

        private static void ProcessHeroSelected(ClientManager MyClient, byte[] PacketData)
        {
            using (MemoryStream ms = new MemoryStream(PacketData))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    br.ReadInt32(); // id_idx
                    br.ReadByte(); // byte null
                    br.ReadByte(); // packet type

                    byte hero_order = br.ReadByte();

                    byte[] hero_namebytes = br.ReadBytes(13);
                    string hero_name = PacketFunctions.ExtractStringFromBytes(hero_namebytes);

                    Console.WriteLine("[REQUEST] Enter game: " + hero_name + ":" + hero_order);
                    SendData.SendResponseSelectChar(MyClient, hero_order);
                }
            }
        } 
    
    }
}
