using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ZoneServer.GameServerManager;

namespace ZoneServer.Network
{
    public struct Packet
    {
        public short Length;
        public byte pub_key;
        public byte[] body;

    }


    public class Receive
    {
        private Queue<XPacket> packets = new Queue<XPacket>();

        public void AddPacket(XPacket packet)
        {
            packets.Enqueue(packet);
        }

        public void Update()
        {
            if(packets.Count > 0)
            {
                ProcessPacket(packets.Dequeue());
            }
        }

        private Packet[] ReadAllPackets(byte[] data, byte privKey)
        {
            List<Packet> packets = new List<Packet>();

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    try
                    {
                        while (br.BaseStream.Position != br.BaseStream.Length)
                        {
                            Packet temp = new Packet();
                            temp.Length = br.ReadInt16();
                            temp.pub_key = br.ReadByte();

                            byte[] content = br.ReadBytes(temp.Length);
                            byte pub_keyindex = XCript.GetPubKeyIndex(temp.pub_key);
                            content = XCript.Decrypt(content, pub_keyindex, privKey);
                            temp.body = content;
                            packets.Add(temp);
                        }
                        return packets.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            return packets.ToArray();
        }

        private void ProcessPacket(XPacket xpacket)
        {
            foreach(Packet packet in ReadAllPackets(xpacket.data, XCript.ZoneServerPrivKey))
            {
                if (packet.body.Length < 10) return;

                using (MemoryStream ms = new MemoryStream(packet.body))
                {
                    using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                    {
                        int client_token = br.ReadInt32();
                        int id_idx = br.ReadInt32();
                        byte hero_order = br.ReadByte();
                        byte packet_type = br.ReadByte();

                        if (!xpacket.client.zs_data.AUTHENTICATED)
                            xpacket.client.zs_data.HERO_ORDER = hero_order;

                        byte[] data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                        HandlePacket(data, packet_type, xpacket.client);
                    }
                }
               //Init.logger.ConsoleLog("Receive data: " + packet.Length, ConsoleColor.Yellow);
            }
        }

        private void HandlePacket(byte[] data, byte type, Client client)
        {
            Init.logger.ConsoleLog("[Client] Received packet type: " + type, ConsoleColor.Yellow);

            switch(type)
            {
                // Inicializa o processo de autenticação
                case 100:
                    ParseAuthentication(data, client);
                    break;

                // Jogo do client foi carregado
                case 105:
                    client.zs_data.LOGGED = true;

                    // enviar todas as informações possíveis ao redor do jogador
                    break;

                // HERO MOVE REQUEST
                case 114:
                    ParseMoveCharacter(data, client);
                    break;
                // HENCH MOVE REQUEST
                case 116:
                    ReceiveMoveHench(data, client);
                    break;
                // NORMAL CHAT REQUEST
                case 125:
                    ParseNormalChatPacket(data, client);
                    break;
           
                // MOVE HENCH Pocket
                case 131:
                    ParseHenchMovePocket(data, client);
                    break;

                // UNEQUIP BATTE HENCH
                case 140:
                    ParseUnequipBattleHench(data, client);
                    break;
                
                default:
                    break;
            }
        }



        // --------------------------------- PARSE PACKETS ------------------------------

        private void ParseAuthentication(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    int id_idx = br.ReadInt32();
                    int token = br.ReadInt32();
                    
                    if(!Init.game.IsValidToken(new GameManager.TokenAccess(id_idx, token, client.zs_data.HERO_ORDER)))
                    {
                        client.Disconnect();
                        return;
                    }

                    if (client.zs_data.AUTHENTICATED) return;
                    client.zs_data.ID_IDX = id_idx;

                    client.zs_data.Create_ZSData();
                    client.zs_data.SendMyInfo();

                    Console.WriteLine("Jogador: " + id_idx + " se autenticou com sucesso! Carregando informações ...");
                }
            }
        }
        private void ParseMoveCharacter(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    if (client.zs_data.hero.IsWalking) return;

                    byte target_x = br.ReadByte();
                    byte target_y = br.ReadByte();
                    client.zs_data.hero.Move(target_x, target_y);

                }
            }
        }

        private void ParseHenchMovePocket(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    byte type = br.ReadByte();
                   
                    byte origin = br.ReadByte();
                    byte dest = br.ReadByte();

                    client.zs_data.hero.MoveHenchPocket(origin, dest);
                    //client.player.ChangeHenchPocket(origin, dest);
                }
            }
        }
        private void ParseUnequipBattleHench(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    byte order = br.ReadByte();

                    client.zs_data.hero.UnequipBattleHench(order);
                }
            }
        }



        private void ReceiveMoveHench(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    /*
                    byte hench_index = br.ReadByte();
                    if (client.player.IsHenchWalking(hench_index)) return;

                    byte targetX = br.ReadByte();
                    byte targetY = br.ReadByte();


                    client.player.HenchWalk(hench_index, targetX, targetY);*/

                }
            }
        }


        private void ParseNormalChatPacket(byte[] data, Client client)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    /*
                    string message = "";

                    byte c;
                    while ((c = br.ReadByte()) != (char)0x00)
                    {
                        message += (char)c;
                    }

                    Console.WriteLine("Normal chat: " + message);
                    HandleMessage(message, client);
                    Init.server.sendManager.SendNormalChat(client.player, message); */
                }
            }
        }

        private void HandleMessage(string message, Client client)
        {
            try
            {
                string[] args = message.Split(' ');
                switch(args[0].ToLower())
                {
                    case "addhench":
                        int id = int.Parse(args[1]);
                        Console.WriteLine("Generating hench: " + id);
                        //client.player.AddHenchInPocket(id);
                        break;

                    case "additem":
                        int item_id = int.Parse(args[1]);
                        int item_count = int.Parse(args[3]);
                        // add item
                        break;
                }

            }
            catch
            {
                return;
            }
        }


    }
}
