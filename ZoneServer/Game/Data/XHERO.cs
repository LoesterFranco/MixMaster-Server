using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class XHERO
    {
        #region HERO_DATA
        public ZS_DATA zs_data;
        public List<XHENCH_INFO> henchs;
        public List<XBATTLE_HENCH> battle_hench;
        public List<CQuest> quests;
        public List<CSkill> skills;
        public CMixSkill mixSkill;
        public List<CItem> items;
        public Stopwatch ZC_TIME;

        public uint id_idx;
        public byte hero_order;
        public double serial;
        public byte Class;
        public string name;
        public byte hero_type;
        public ushort now_zone_idx;
        public byte now_zone_x;
        public byte now_zone_y;
        public byte moving_to_x;
        public byte moving_to_y;
        public ushort init_pos_layer;
        public ushort revive_zone_idx;
        public ushort baselevel;
        public uint gold;
        public uint attr;
        public long exp;
        public byte speed_move;
        public ushort speed_attack;
        public ushort speed_skill;
        public uint str;
        public uint dex;
        public uint aim;
        public uint luck;
        public ushort ap;
        public ushort dp;
        public ushort hc;
        public ushort hd;
        public uint hp;
        public uint mp;
        public uint maxhp;
        public uint maxmp;
        public uint abil_freepoint;
        public ushort res_fire;
        public ushort res_water;
        public ushort res_earth;
        public ushort res_wind;
        public ushort res_devil;
        public byte ign_att_cnt;
        public DateTime regdate;
        public ushort avatar_head;
        public ushort avatar_body;
        public ushort avatar_foot;
        public uint return_time;
        public byte status;
        public DateTime status_time;
        public ushort nickname;
        public DateTime last_logout_time;
        public uint skill_point;
        public byte login;

        #endregion

        #region constructor
        public XHERO(ZS_DATA data)
        {
            this.zs_data = data;
            this.henchs = new List<XHENCH_INFO>();
            this.battle_hench = new List<XBATTLE_HENCH>();
            this.quests = new List<CQuest>();
            this.skills = new List<CSkill>();
            this.mixSkill = new CMixSkill(this);
            this.items = new List<CItem>();
            this.ZC_TIME = new Stopwatch();
            this.ZC_TIME.Start();
            if (data.GetConnectionAtributes().Connected)
            {
                Console.WriteLine("[XHERO] Connected!!");
            }

        }
        #endregion

        #region HERO_FUNCTIONS
        public void AddHench(XHENCH_INFO hench)
        {
            this.henchs.Add(hench);
        }
        public void RemoveHench(XHENCH_INFO hench)
        {
            this.henchs.Remove(hench);
        }
        public void AsyncRemoveHench(XHENCH_INFO hench)
        {
            if (hench == null) return;
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)145); // packet type
                    Console.WriteLine("remover hench: " + hench.hench_order);
                    bw.Write((byte)hench.hench_order);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }

            this.RemoveHench(hench);
        }

        public void AsyncRemoveBattleHench(XBATTLE_HENCH hench)
        {
            if (hench == null) return;
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)83); // packet type
                    Console.WriteLine("remover battle hench: " + hench.hench_base.hench_order);
                    bw.Write((byte)hench.hench_base.hench_order);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }

            this.RemoveBattleHench(hench);
        }

        public void AddBattleHench(XBATTLE_HENCH hench)
        {
            this.battle_hench.Add(hench);
        }
        public void RemoveBattleHench(XBATTLE_HENCH hench)
        {
            this.battle_hench.Remove(hench);
        }

        public void AddItem(CItem item)
        {
            this.items.Add(item);
        }
        public void RemoveItem(CItem item)
        {
            this.items.Remove(item);
        }

        public void AddQuest(CQuest quest)
        {
            this.quests.Add(quest);
        }
        public void RemoveQuest(CQuest quest)
        {
            this.quests.Remove(quest);
        }

        public void AddSkill(CSkill skill)
        {
            this.skills.Add(skill);
        }
        public void RemoveSkill(CSkill skill)
        {
            this.skills.Remove(skill);
        }

        public int GetElapsedTime()
        {
            return (int)ZC_TIME.ElapsedMilliseconds;
        }

        #region MOVE_MANAGER
        private long curMoveTime = 0;
        private long waitMoveTime = 0;
        public bool IsWalking = false;

        public void ProcMove(long gameTime)
        {
            if (!IsWalking) return;

            curMoveTime += gameTime;
            if(curMoveTime >= waitMoveTime)
            {
                curMoveTime = 0;
                waitMoveTime = 0;
                now_zone_x = moving_to_x;
                now_zone_y = moving_to_y;
                IsWalking = false;
                this.SendPositionUpdate();
            }
        }
        #endregion



        #endregion


        #region HERO_LOAD

        public void LoadThisHero()
        {
            Init.dbManager.gameData.LoadMyHero(this);
        }
        public void LoadMyHenchs()
        {
            Init.dbManager.gameData.LoadMyHenchs(this);
        }
        public void LoadMyQuests()
        {
            Init.dbManager.gameData.LoadMyQuests(this);
        }
        public void LoadMySkills()
        {
            Init.dbManager.gameData.LoadMySkills(this);
        }
        public void LoadMixSkill()
        {
            Init.dbManager.gameData.LoadMixSkill(this);
        }
        public void LoadItems()
        {
            Init.dbManager.gameData.LoadMyItems(this);
        }

        #endregion


        #region SEND_DATA
        private void SendHeroPacket()
        {
            Console.WriteLine("Enviando dados do hero ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)0x66); // packet type

                    bw.Write((uint)Init.proc.GetTickCount());

                    bw.Write((byte)31); // max character name length provable
                    bw.Write((byte)0x00); // begin string
                    for (int i = 0; i < name.Length; i++)
                    {
                        bw.Write(name[i]);
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
                    bw.Write((byte)hero_type); // hero type
                    bw.Write((byte)now_zone_idx); // hero zone id
                    bw.Write((byte)0x00); // unknow
                    bw.Write((byte)now_zone_x); // hero pos_x
                    bw.Write((byte)now_zone_y); // hero pos_y
                    bw.Write((byte)baselevel); // hero lv
                    bw.Write((byte)0x00); // unknow

                    // hero
                    bw.Write((int)gold); // hero gp
                    bw.Write((short)avatar_head);
                    bw.Write((long)exp);
                    bw.Write((long)Init.game.sdata.lvUserInfo[baselevel].LvUpExp); // back lv
                    bw.Write((long)Init.game.sdata.lvUserInfo[baselevel + 1].LvUpExp); // next lv
                    bw.Write((byte)speed_move); // hero speed move
                    bw.Write((short)0x00); // unknow
                    bw.Write((short)str); // Energia
                    bw.Write((short)dex); // Agilitade
                    bw.Write((short)aim); // Exaltidao
                    bw.Write((short)luck); // sorte 
                    bw.Write((short)ap); // ap
                    bw.Write((short)dp); // dp
                    bw.Write((short)hc); // HC
                    bw.Write((short)hd); // not verificated
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

                    bw.Write((short)abil_freepoint); // free points stats
                    bw.Write((int)id_idx); // hero id_idx
                    bw.Write((byte)nickname);
                    bw.Write((byte)nickname);
                    bw.Write((byte)0x00); // clean

                    // guild
                    bw.Write((Int16)0x00); // unk
                    bw.Write((byte)0x03); // guild id
                    string guild_name = "HERCULES";
                    bw.Write((byte)0x00); // begin string

                    for (int i = 0; i < guild_name.Length; i++)
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

                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
       
        public XHENCH_INFO GetOccupiedHenchPocket(byte order)
        {
            for (int i = 0; i < henchs.Count; i++)
                if (henchs[i].hench_order == order && henchs[i].position == 1)
                    return henchs[i];

            return null;
        }

        public XBATTLE_HENCH GetOcupiedBattleHenchPocket(byte order)
        {
            for (int i = 0; i < battle_hench.Count; i++)
                if (battle_hench[i].hench_base.hench_order == order && battle_hench[i].hench_base.position == 0)
                    return battle_hench[i];

            return null;
        }

        public int GetEmptyHenchPocket()
        {
            int value = 0;

            List<XHENCH_INFO> newList = henchs.OrderBy(order => order.hench_order).ToList();
            for(int i = 0; i < newList.Count; i++)
            {
                if (newList[i].hench_order != (byte)i)
                {
                    Console.WriteLine("order: " + newList[i].hench_order);
                    value = i;
                    return value;
                }
                else
                {
                    if(i == newList.Count)
                    {
                        if(i < 19)
                        {
                            value = i;
                            return value;
                        }
                    }
                }
               
            }


            return value;
        }

        public void MoveHenchPocket(byte initial, byte end)
        {
            if (initial < 0 || initial > 19) return;
            if (end < 0 || end > 19) return;
            Console.WriteLine("Move Hench pocket request: " + initial + " to " + end);


            if (initial == end) return;
            XHENCH_INFO hench1 = GetOccupiedHenchPocket(initial);
            Console.WriteLine("Hench1: " + hench1.hench_order);
            if (hench1 == null) return;

            XHENCH_INFO hench2 = GetOccupiedHenchPocket(end);
            if(hench2 == null) // no have hench
            {
                XHENCH_INFO clone1 = (XHENCH_INFO)hench1.Clone();
                clone1.hench_order = end;

                AsyncRemoveHench(hench1);

                henchs.Add(clone1);
                clone1.SendHenchInfoPacket();
               
                return;
            }
            else
            {
                byte hench1_order = hench1.hench_order;
                hench1.hench_order = hench2.hench_order;
                hench2.hench_order = hench1_order;

                hench1.SendHenchInfoPacket();
                hench2.SendHenchInfoPacket();
                return;
            }
        }
        public void UnequipBattleHench(byte order)
        {
            if (order < 0 || order > 19) return;

            this.SendUnequipBattleHench(order);

        }


        private void SendHenchBattleInfoPacket(XBATTLE_HENCH battle_hench)
        {
            Console.WriteLine("Enviando Hench Pocket ...");
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    XHENCH_INFO hench = battle_hench.hench_base;

                    bw.Write((byte)103); // packet type
                    bw.Write((byte)hench.position); // position
                    bw.Write((byte)hench.hench_order); // hench order
                    bw.Write((short)hench.HenchID);

                    byte[] hench_name = new byte[25];
                    for (int i = 0; i < hench.HenchName.Length; i++)
                    {
                        hench_name[i] = (byte)hench.HenchName[i];
                    }
                    bw.Write(hench_name);


                    bw.Write((byte)hench.sex);

                    if (hench.IsDead)
                        bw.Write((byte)(0x01));
                    else
                        bw.Write((byte)0x00);


                    bw.Write((short)hench.mixnum); // talvez


                    bw.Write((short)hench.hench_lvmax);
                    bw.Write((short)hench.hench_lv);
                    bw.Write((long)hench.hench_exp); // current exp
                    bw.Write((long)hench.hench_exp); // exp base
                    bw.Write((long)hench.hench_exp + 400); // exp next lv

                    bw.Write((byte)0x03); // speed move
                    bw.Write((byte)0x00); // speed attack
                    bw.Write((byte)0x00); // speed skill
                    bw.Write((short)hench.str);
                    bw.Write((short)hench.dex);
                    bw.Write((short)hench.aim);
                    bw.Write((short)hench.luck);
                    bw.Write((short)hench.ap); // talvez
                    bw.Write((short)hench.dp); // talvez
                    bw.Write((short)hench.hc); // talvez
                    bw.Write((short)hench.hd); // talvez
                    bw.Write((short)hench.race_val);
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


                    len = (int)bw.BaseStream.Length;
                    Console.WriteLine("Hench packet len: " + len);
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        private void SendSkillsInfoPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)74); // packet type
                    int count = skills.Count;
                    bw.Write((byte)count);
                    for (int i = 0; i < count; i++)
                    {

                        bw.Write((byte)skills[i].skillID);
                        bw.Write((byte)skills[i].skillLevel);
                        bw.Write((int)skills[i].skillPoint);
                        bw.Write((byte)2);
                        bw.Write((byte)240);
                    }
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        private void SendMixSKillInfoPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)60); // packet type
                    bw.Write((byte)10); // MAX MIX SKILL

                    bw.Write((Int16)mixSkill.MixSkill1);
                    bw.Write((Int16)mixSkill.MixSkill2);
                    bw.Write((Int16)mixSkill.MixSkill3);
                    bw.Write((Int16)mixSkill.MixSkill4);
                    bw.Write((Int16)mixSkill.MixSkill5);
                    bw.Write((Int16)mixSkill.MixSkill6);
                    bw.Write((Int16)mixSkill.MixSkill7);
                    bw.Write((Int16)mixSkill.MixSkill8);
                    bw.Write((Int16)mixSkill.MixSkill9);
                    bw.Write((Int16)mixSkill.MixSkill10);

                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        public void SendUnequipBattleHench(byte order)
        {
            XBATTLE_HENCH hench = GetOcupiedBattleHenchPocket(order);
            if (hench == null) return;
            hench.SendRefreshStatus(this, order);
            int empty = GetEmptyHenchPocket();
            if (empty == -1) return;

            XHENCH_INFO hench_info = (XHENCH_INFO)hench.hench_base.Clone();
            this.AsyncRemoveBattleHench(hench);

            hench_info.hench_order = (byte)empty;
            hench_info.position = 1;

            hench_info.SendHenchInfoPacket();
            henchs.Add(hench_info);
        }

        public void SendMySkillsPoints()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)70); // packet type
                    bw.Write((Int16)skill_point);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        private void SendItemsPacket()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)104); // packet type
                    int itemsCount = items.Count;
                    bw.Write((byte)itemsCount);
                    for (int i = 0; i < itemsCount; i++)
                    {
                        // item id (ushort)
                        bw.Write((byte)items[i].socket_num);
                        bw.Write((ushort)items[i].id);
                        bw.Write((ushort)items[i].count);
                        bw.Write((byte)items[i].opt);
                        bw.Write((byte)items[i].opt_level);
                        bw.Write((uint)items[i].duration);
                        bw.Write((byte)items[i].synergy);
                        bw.Write((byte)items[i].synergy_level);
                    }
                    bw.Write((byte)0); // items using count
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        private void SendAllQuestsProgress()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((byte)0x43); // packet type
                    bw.Write((short)quests.Count);
                    Console.WriteLine("Questlogcount: " + quests.Count);
                    for (int i = 0; i < quests.Count; i++)
                    {
                        bw.Write((short)quests[i].quest_index);
                        bw.Write((byte)quests[i].quest_state);
                    }
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }
        }
        public void Move(byte targetX, byte targetY)
        {
            int distance = GameManager.GetDistance(targetX, targetY, now_zone_x, now_zone_y);
            int wait_time = (int)Math.Floor((double)500 / speed_move);
            int total_wait = wait_time * distance;

            moving_to_x = targetX;
            moving_to_y = targetY;

            IsWalking = true;
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)117); // packet type
                    bw.Write((byte)now_zone_x);
                    bw.Write((byte)now_zone_y);
                    bw.Write((byte)moving_to_x);
                    bw.Write((byte)moving_to_y);
                    int time = GetElapsedTime();
                    bw.Write((int)time);
                    Console.WriteLine("This time: " + time);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);

                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);
            }

        }
        public void SendPositionUpdate()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)119); // packet type
                    bw.Write((byte)14);
                    bw.Write((byte)1);
                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Console.WriteLine("Posiçao confirmada!");
                Init.server.sendManager.MakePacketAndSend(this.zs_data.GetConnectionAtributes(), buffer);

            }
        }
        #endregion


        public void SendThisHero()
        {
            this.SendHeroPacket();
        }
        public void SendThisHenchs()
        {
            for(int i = 0; i < this.henchs.Count; i++)
            {
                henchs[i].SendHenchInfoPacket();
            }

            for(int i = 0; i < this.battle_hench.Count; i++)
            {
                battle_hench[i].hench_base.SendHenchInfoPacket();
            }
        }
        public void SendThisSkills()
        {
            this.SendSkillsInfoPacket();
        }
        public void SendThisMixSkill()
        {
            this.SendMixSKillInfoPacket();
        }
        public void SendThisItems()
        {
            this.SendItemsPacket();
        }
        public void SendThisQuestLog()
        {
            this.SendAllQuestsProgress();
        }

       
    }
}
