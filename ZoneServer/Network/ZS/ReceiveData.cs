using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ZoneServer.Network.ZS
{
    public class GMSToken
    {
        public int id_idx;
        public int token;
        public byte hero_order;
    }

    public class ReceiveData
    {
        public static List<GMSToken> Tokens = new List<GMSToken>();
        public static bool IsIDXInToken(int id_idx)
        {
            for(int i=0; i < Tokens.Count; i++)
            {
                if(Tokens[i].id_idx == id_idx)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidToken(int id_idx, int token, byte hero_order)
        {
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (Tokens[i].id_idx == id_idx && Tokens[i].token == token && Tokens[i].hero_order == hero_order)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Handle_Client_Packet(Client MyClient, byte[] data)
        {
            if (MyClient == null) { return; }
            if (MyClient.socket == null) { return; }
            if (!MyClient.socket.Connected) { return; }

            if(data[0] == 0) { return; }

            byte[] PacketDecrypted = PacketFunctions.GetPacketDataDecrypted(data);

            string message = string.Join(", ", data);
            Console.WriteLine($"Received: " + message);
            using (MemoryStream ms = new MemoryStream(PacketDecrypted))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    int ClientToken = br.ReadInt32();
                    int id_idx = br.ReadInt32();
                    if(!IsIDXInToken(id_idx))
                    {
                        Console.WriteLine("id_idx não encontrada no token!");
                        return;
                    }
                    byte hero_order = br.ReadByte();
                    byte PacketType = br.ReadByte();
                    Console.WriteLine("Packet: " + PacketType);
                    switch(PacketType)
                    {
                        case 100: // auth
                            #region auth
                            int id_idx2 = br.ReadInt32();
                            int token = br.ReadInt32();

                            if (!IsValidToken(id_idx2, token, hero_order))
                            {
                                Console.WriteLine("Token is invalid: " + id_idx2 + ":" + token);
                                return;
                            }

                            MyClient.account.id_idx = id_idx2;
                            MyClient.account.token = token;
                            MyClient.account.IsValid = true;
                            Console.WriteLine("Token is valid: " + id_idx2 + ":" + token);

                            // hero info
                            SendData.SendInfo_hero(MyClient, hero_order);

                            // quest log info
                            SendData.SendInfo_QuestLog(MyClient);

                            // henchs info
                            List<Structs.gamedata.u_hench> Hench = Database.Gamedata.GetHenchs(MyClient);
                            MyClient.data.MyHenchs = Hench;

                            for(int i =0; i < MyClient.data.MyHenchs.Count; i++)
                            {
                                SendData.SendInfo_HenchPocket(MyClient, MyClient.data.MyHenchs[i]);
                            }


                            Console.WriteLine("Henchs: " + Hench.Count);


                            Console.WriteLine("Foi enviado informações do hero!");
                            #endregion auth
                            break;

                        case 105: // confirm received all data
                            break;
                        case 114:
                            ReceiveMoveCharacter(MyClient, PacketDecrypted);
                            break;
                        
                            break;
                        case 125:
                            ReceiveNormalChat(MyClient, PacketDecrypted);
                            break;
                        default:
                            Console.WriteLine("Packet unknow: " + PacketType);
                            break;
                    }
                }
            }
            
            // SendData.Send(MyClient, data);


        }

        private static void ReceiveMoveCharacter(Client client, byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.ReadBytes(10);
                    byte pos_x = br.ReadByte();
                    byte pos_y = br.ReadByte();
                    client.data.Hero.moving_to_x = pos_x;
                    client.data.Hero.moving_to_y = pos_y;
                    // send to client is walking status
                    SendData.SendStatusIsWalking(client);
                   
                    /*  Calcular a velocidade de movimento
                     * 
                     * */

                    // enviar a posição atualizado
                    SendData.SendPositionUpdated(client);

                    client.data.Hero.now_zone_x = pos_x;
                    client.data.Hero.now_zone_y = pos_y;
                }
            }
        }

        private static void ReceiveNormalChat(Client Player, byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.ReadInt32();
                    br.ReadInt32();
                    br.ReadByte();
                    br.ReadByte();

                    string message = "";

                    char c;
                    while ((c = br.ReadChar()) != (char)0x00)
                    {
                        message += c;
                    }

                    SendData.SendNormalChat(Player, message);
                    Console.WriteLine("Message: " + message);

                }
            }
        }

        private static void Handle_Client_Packet(int packet_type)
        {
            switch(packet_type)
            {
                case 63: // request item mix core
                    break;
                case 64: // request item mix rate, check rate mix
                    break;
                case 72: // Skill level up
                    break;
                case 77: // hench equip off
                    break;
                case 79: // hench equip on
                    break;
                case 81: // ZS_Castle war
                    break;
                case 84: // ZS_Castle warp
                    break;
                case 85: // ZS_Castle change tax rate
                    break;
                case 92: // Speaker chat
                    break;
                case 96: // Request npc barter item
                    break;
                case 97: // request npc barter gambling item
                    break;
                case 98: // Advertisement Event
                    break;
                case 99: // Create Item System
                    break;
                case 114: // Request Move All
                    break;
                case 115: // Request Move Hero
                    break;
                case 116: // Request Move hench
                    break;
                case 122: // Request MR Attack
                    break;
                case 125: // Default chat
                    break;
                case 126: // Whisper chat
                    break;
                case 128: // Command chat
                    break;
                case 129: // Request pickup item
                    break;
                case 130: // Request put item
                    break;
                case 131: // Request move pocket object
                    break;
                case 132: // Request Use Pocket Item
                    break;
                case 136: // Request equip item
                    break;
                case 137: // Request equip off item
                    break;
                case 140: // Request Make Core
                    break;
                case 141: // Request Make Hench
                    break;
                case 147: // Request Put Core
                    break;
                case 148: // Request put Gold
                    break;
                case 152: // Request Use Skill
                    break;
                case 155: // Request Npc Contact
                    break;
                case 157: // Request npc buy item
                    break;
                case 158: // Request npc sell item
                    break;
                case 159: // Request npc revive core
                    break;
                case 160: // Request npc mix core
                    break;
                case 161: // Request npc change hench name
                    break;
                case 166: // Request npc quest
                    break;
                case 168: // Request trade
                    break;
                case 169: // Request accept trade
                    break;
                case 171: // Request Trade add object
                    break;
                case 173: // Request Trade OK
                    break;
                case 174: // Request Trade Conclute
                    break;
                case 175: // Request Trade Cancel
                    break;
                case 178: // Go to select character
                    break;
                case 187: // Request character Status up
                    break;
                case 191: // Request Hero sit
                    break;
                case 192: // Request Dead Return
                    break;
                case 193: // Request Mix Core
                    break;
                case 195: // Request Enchant
                    break;
                case 197: // Request revive manage
                    break;
                case 199: // Request npc rerive core all
                    break;
                case 200: // Guild system
                    break;
                case 201: // Party system
                    break;
                case 202: // Speaker Chat
                    break;
                case 239: // Request Quest item
                    break;
                case 245: // Request Messenger
                    break;
                case 247: // Request StoreHouse
                    break;
                case 249: // Request Sale
                    break;
                case 251: // Request Gambling
                    break;
                case 253: // Request Take
                    break;
                case 255: // Request Trace
                    break;

            }
        }
    }
}
