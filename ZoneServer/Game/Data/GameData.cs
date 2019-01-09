using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager.Data
{
    public class GameData
    {

        // Dados dos jogadores logados
        // Todos os dados da configuração do servidor; Database
        // NPC's
        // MOB's
        // MAP's
        // ...

        public List<XQuestLog> questLog;
        public List<XHero> hero;
        public List<XHench> hench;
        public List<XGuild> guild;
        public List<XGuildMember> guildMember;

        public GameData()
        {
            questLog = new List<XQuestLog>();
            hero = new List<XHero>();
            hench = new List<XHench>();
            guild = new List<XGuild>();
            guildMember = new List<XGuildMember>();
        }


        public struct XQuestLog
        {
            public ushort quest_index;
            public byte quest_state;

            public void SetStatus(byte status)
            {
                quest_state = status;
            }
        }

        public struct XHero
        {
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
        }

        public struct XHench
        {
            public bool IsWalking;
            public long walk_wait;
            public long current_wait;

            public byte target_x;
            public byte target_y;

            public byte posX;
            public byte posY;

            public void SetPosition(byte posx, byte posy)
            {
                this.posX = posx;
                this.posY = posy;
            }

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
        }

        public struct XGuild
        {
            public ushort GuildIdx;
            public string Name;
            public string Info;
            public string Cert;
            public DateTime EstablishDate;
            public ushort LimitCount;
            public byte Status;
            public DateTime MarkRegDate;
            public byte MarkRegCnt;
            public DateTime Dissolution;
            public uint gold;
            public ushort HiringIdx;
            public DateTime CertDate;
            public DateTime InfoDate;
        }

        public struct XGuildMember
        {
            public uint HeroIdx;
            public byte HeroOrder;
            public ushort GuildIdx;
            public ushort MemberID;
            public ushort Grade;
            public uint Authority;
            //public DateTime UpgradeDate;
            public string Memo;
        }

        public struct XItem
        {
            public uint id;
            public byte socket_type;
            public byte socket_num;
            public ushort count;
            public byte opt;
            public byte opt_level;
            public uint duration;
            public DateTime last_check_time;
            public byte synergy;
            public byte synergy_level;
        }

        public struct XSkill
        {
            public byte skillID;
            public byte skillLevel;
            public ushort skillPoint;
            public DateTime learningDate;
        }

        public struct XMixSkill
        {
            public ushort MixSkill1;
            public ushort MixSkill2;
            public ushort MixSkill3;
            public ushort MixSkill4;
            public ushort MixSkill5;
            public ushort MixSkill6;
            public ushort MixSkill7;
            public ushort MixSkill8;
            public ushort MixSkill9;
            public ushort MixSkill10;
        }
    }
}
