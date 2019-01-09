using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class XHENCH_INFO
    {
        private XHERO hero;

        public bool IsWalking;
        public long walk_wait;
        public long current_wait;
        public byte target_x;
        public byte target_y;
        public byte posX;
        public byte posY;
        public ushort HenchID;
        public string HenchName;
        public byte position;
        public byte hench_order;
        public byte sex;
        public bool IsDead;
        public byte mixnum;
        public ushort hench_lvmax;
        public ushort hench_lv;
        public long hench_exp;
        public long hench_exp_backlevel;
        public long hench_exp_nextlevel;
        public byte enchant_grade;
        public ushort str;
        public ushort dex;
        public ushort aim;
        public ushort luck;
        public ushort ap;
        public ushort dp;
        public ushort hc;
        public ushort hd;
        public uint hp;
        public uint mp;
        public uint max_hp;
        public uint max_mp;
        public ushort item0_idx;
        public uint item0_duration;
        public ushort item1_idx;
        public uint item1_duration;
        public ushort item2_idx;
        public uint item2_duration;
        public byte growthtype;
        public ushort race_val;
        public byte item_slot_total;
        public int duration;


        public void SendHenchInfoPacket()
        {
            Console.WriteLine("Enviando Hench Pocket ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)103); // packet type
                    bw.Write((byte)position); // position
                    bw.Write((byte)hench_order); // hench order
                    bw.Write((short)HenchID);

                    byte[] hench_name = new byte[25];
                    for (int i = 0; i < HenchName.Length; i++)
                    {
                        hench_name[i] = (byte)HenchName[i];
                    }
                    bw.Write(hench_name);


                    bw.Write((byte)sex);

                    if (IsDead)
                        bw.Write((byte)(0x01));
                    else
                        bw.Write((byte)0x00);


                    bw.Write((short)mixnum); // talvez


                    bw.Write((short)hench_lvmax);
                    bw.Write((short)hench_lv);
                    bw.Write((long)hench_exp); // current exp
                    bw.Write((long)hench_exp); // exp base
                    bw.Write((long)hench_exp + 400); // exp next lv

                    bw.Write((byte)0x03); // speed move
                    bw.Write((byte)0x00); // speed attack
                    bw.Write((byte)0x00); // speed skill
                    bw.Write((short)str);
                    bw.Write((short)dex);
                    bw.Write((short)aim);
                    bw.Write((short)luck);
                    bw.Write((short)ap); // talvez
                    bw.Write((short)dp); // talvez
                    bw.Write((short)hc); // talvez
                    bw.Write((short)hd); // talvez
                    bw.Write((short)race_val);
                    bw.Write((short)hp);
                    bw.Write((short)mp);
                    bw.Write((short)max_hp);
                    bw.Write((short)max_mp);
                    bw.Write((byte)growthtype);
                    bw.Write((byte)enchant_grade);
                    bw.Write((long)duration);
                    bw.Write((byte)0x00); // unk
                    bw.Write((byte)item_slot_total);
                    bw.Write((short)item0_idx);
                    bw.Write((int)item0_duration);
                    bw.Write((short)item1_idx);
                    bw.Write((int)item1_duration);
                    bw.Write((short)item2_idx);
                    bw.Write((int)item2_duration);


                    len = (int)bw.BaseStream.Length;
                    Console.WriteLine("Hench packet len: " + len);
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                Init.server.sendManager.MakePacketAndSend(hero.zs_data.GetConnectionAtributes(), buffer);
            }
        }

        public XHENCH_INFO(XHERO hero)
        {
            this.hero = hero;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


    }
}
