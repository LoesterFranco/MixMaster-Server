using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.Structs
{
    public class gamedata
    {
        public class QuestLog
        {
            public short quest_index;
            public byte quest_state;
        }

        public class u_hench
        {
            public short HenchID;
            public string HenchName;
            public byte position;
            public byte hench_order;
            public byte sex;
            public bool IsDead;
            public byte mixnum;

            public byte hench_lvmax;
            public byte hench_lv;
            public long hench_exp;
            public long hench_exp_backlevel;
            public long hench_exp_nextlevel;
            public byte enchant_grade;
            public short str;
            public short dex;
            public short aim;
            public short luck;
            public short ap;
            public short dp;
            public short hc;
            public short hd;
            public short hp;
            public short mp;
            public short max_hp;
            public short max_mp;
            public short item0_idx;
            public int item0_duration;
            public short item1_idx;
            public int item1_duration;
            public short item2_idx;
            public int item2_duration;
            public byte growthtype;
            public byte race_val;
            public byte item_slot_total;
            
            public int duration;
           
        }

    }
}
