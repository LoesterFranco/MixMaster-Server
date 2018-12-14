using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ZoneServer.Crypo;
using System.IO;

namespace ZoneServer.Network.ZS
{
    public class SendData
    {
        public static void Send(Client MyClient, byte[] data)
        {
            try
            {
                Socket socket = MyClient.socket;
                if (socket.Connected)
                {
                    socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
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
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch
            {
                return;
            }
        }

        private static void MakePacketAndSend(Client MyClient, byte[] content)
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
                    bw.Write(XCRYPT.Encrypt(content, XCRYPT.GetPubKeyIndex(RandomPubKey), XCRYPT.ZoneServerPrivKey));
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                Send(MyClient, buffer);
            }

        }

        public static void SendHello(Client MyClient)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write(0x65); // packet type
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);

            }
        }



        #region send init hero_info
        public static void SendInfo_hero(Client MyClient, byte hero_order)
        {
            Console.WriteLine("Enviando dados do hero ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)0x66); // packet type
                    XHERO hero = Database.Gamedata.LoadHero(MyClient.account.id_idx, hero_order);
                    MyClient.data.Hero = hero;
                    bw.Write(new byte[] { 0xDB, 0x42, 0x88, 0x00}); // unknow
                    bw.Write((byte)21); // max character name length provable
                    bw.Write((byte)0x00); // begin string
                    for(int i=0; i < hero.name.Length; i++)
                    {
                        bw.Write(hero.name[i]);
                    }
                    bw.Write((byte)0x00); // end string
                    // map
                    bw.Write((byte)0xf7); // unk
                    bw.Write((byte)0xab); // unk
                    bw.Write((byte)0x09); // unk
                    bw.Write((byte)0x6a); // unk
                    bw.Write((byte)0xF7); // unk
                    bw.Write((byte)0xC0); // unk
                    bw.Write((byte)0x09); // unk
                    bw.Write((byte)0x25); // unknow
                    bw.Write((byte)hero.hero_type); // hero type
                    bw.Write((byte)hero.now_zone_idx); // hero zone id
                    bw.Write((byte)0x00); // unknow
                    bw.Write((byte)hero.now_zone_x); // hero pos_x
                    bw.Write((byte)hero.now_zone_y); // hero pos_y
                    bw.Write((byte)hero.baselevel); // hero lv
                    bw.Write((byte)0x00); // unknow

                    // hero
                    bw.Write((int)hero.gold); // hero gp
                    bw.Write((short)hero.avatar_head);
                    bw.Write((long)hero.exp);
                    bw.Write((long)Data.SData.LvUserInfo[hero.baselevel].LvUpExp); // back lv
                    bw.Write((long)Data.SData.LvUserInfo[hero.baselevel + 1].LvUpExp); // next lv
                    bw.Write((byte)hero.speed_move); // hero speed move
                    bw.Write((short)0x00); // unknow
                    bw.Write((short)hero.str); // Energia
                    bw.Write((short)hero.dex); // Agilitade
                    bw.Write((short)hero.aim); // Exaltidao
                    bw.Write((short)hero.luck); // sorte 
                    bw.Write((short)hero.ap); // ap
                    bw.Write((short)hero.dp); // dp
                    bw.Write((short)hero.hc); // HC
                    bw.Write((short)hero.hd); // not verificated
                    bw.Write((byte)0x00); // unknow
                    bw.Write((byte)110);
                    bw.Write((short)310); // hero hp
                    bw.Write((short)320); // hero mp
                    bw.Write((short)330); // hero max hp
                    bw.Write((short)340); // hero max mp

                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk
                    bw.Write((short)0x00); // unk

                    bw.Write((short)hero.abil_freepoint); // free points stats
                    bw.Write((int)hero.id_idx); // hero id_idx
                    bw.Write((byte)hero.nickname);
                    bw.Write((byte)hero.nickname);
                    bw.Write((byte)0x00); // clean

                    // guild
                    bw.Write((Int16)0x00); // unk
                    bw.Write((byte)0x03); // guild id
                    string guild_name = "HERCULES";
                    bw.Write((byte)0x00); // begin string

                    for(int i =0; i < guild_name.Length; i++)
                    {
                        bw.Write((byte)guild_name[i]);
                    }
                    bw.Write((byte)0x00); // end string

                    byte guild_members = 12;
                    bw.Write((byte)guild_members);

                    bw.Write((byte)0x00);
                    
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                Console.WriteLine("Len send hero info: " + len);
                MakePacketAndSend(MyClient, buffer);

            }
        }

        public static void SendInfo_QuestLog(Client MyClient)
        {
            Console.WriteLine("Enviando QuestLog ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    List<Structs.gamedata.QuestLog> Quests = Database.Gamedata.GetQuestsLog(MyClient);
                    MyClient.data.QuestsLog = Quests;

                    bw.Write((byte)0x43); // packet type
                    bw.Write((short)Quests.Count);
                    Console.WriteLine("Questlogcount: " + Quests.Count);
                    for(int i =0; i < Quests.Count; i++)
                    {
                        bw.Write((short)Quests[i].quest_index);
                        bw.Write((byte)Quests[i].quest_state);
                    }
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);
            }
           
        }


        public static void SendInfo_HenchPocket(Client MyClient, Structs.gamedata.u_hench hench)
        {
            Console.WriteLine("Enviando Hench Pocket ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)0x67);
                    bw.Write((byte)0x01);
                    bw.Write((byte)hench.hench_order);
                    bw.Write((short)hench.HenchID);

                    byte[] hench_name = new byte[25];
                    for(int i = 0; i < hench.HenchName.Length; i++)
                    {
                        hench_name[i] = (byte)hench.HenchName[i];
                    }
                    bw.Write(hench_name);
                    if (hench.IsDead)
                    {
                        bw.Write((byte)(0x01));
                    }
                    else { bw.Write((byte)0x00); }
                    bw.Write((byte)0x00); // unk

                    bw.Write((byte)0x01); // unk
                    bw.Write((byte)0x00); // unk
                    bw.Write((short)hench.hench_lvmax);
                    bw.Write((short)hench.hench_lv);
                    bw.Write((long)hench.hench_exp);
                    bw.Write((long)hench.hench_exp_backlevel);
                    bw.Write((long)hench.hench_exp_nextlevel);

                    bw.Write((short)0x03); // unk
                    bw.Write((byte)0x00); // unk
                    bw.Write((short)hench.str);
                    bw.Write((short)hench.dex);
                    bw.Write((short)hench.aim);
                    bw.Write((short)hench.luck);
                    bw.Write((short)hench.ap);
                    bw.Write((short)hench.dp);
                    bw.Write((short)hench.hc);
                    bw.Write((short)hench.hd);
                    bw.Write((short)hench.hp);
                    bw.Write((short)hench.dex);
                    bw.Write((short)hench.hp);
                    bw.Write((short)hench.mp);
                    bw.Write((short)hench.max_hp);
                    bw.Write((short)hench.max_mp);
                    bw.Write((byte)hench.growthtype);
                    bw.Write((byte)hench.enchant_grade);
                    bw.Write((long)hench.duration);
                    bw.Write((byte)0x00); // unk
                    bw.Write((byte)hench.item_slot_total);
                    bw.Write((short)hench.item0_idx);
                    bw.Write((int)hench.item0_duration);
                    bw.Write((short)hench.item1_idx);
                    bw.Write((int)hench.item1_duration);
                    bw.Write((short)hench.item2_idx);
                    bw.Write((int)hench.item2_duration);
                    bw.Write((short)hench.HenchID);


                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                MakePacketAndSend(MyClient, buffer);
            }
        }


        #endregion send init hero_info

        public static void SendStatusIsWalking(Client Player)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)117);
                    bw.Write((byte)Player.data.Hero.now_zone_x);
                    bw.Write((byte)Player.data.Hero.now_zone_y);
                    bw.Write((byte)Player.data.Hero.moving_to_x);
                    bw.Write((byte)Player.data.Hero.moving_to_y);
                    bw.Write((int)0);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Console.WriteLine("Enviando confirmação de posição!");
                MakePacketAndSend(Player, buffer);

            }
        }


        public static void SendNormalChat(Client Player, string message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)125);
                    bw.Write((byte)15);
                    bw.Write((byte)0);
                    foreach(char c in Player.data.Hero.name)
                    {
                        bw.Write((byte)c);
                    }
                    
                    bw.Write((byte)0);
                    foreach (char c in message)
                    {
                        bw.Write((byte)c);
                    }
                    bw.Write((byte)0);

                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Console.WriteLine("Chat Mensagem enviado");
                MakePacketAndSend(Player, buffer);
            }
        }

        public static void SendPositionUpdated(Client Player)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)119);
                    bw.Write((byte)19);
                    bw.Write((byte)8);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Console.WriteLine("Posiçao confirmada!");
                MakePacketAndSend(Player, buffer);
            }
        }

    }
}
