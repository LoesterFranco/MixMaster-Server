using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager.Data
{
    public class SData
    {
        public List<LvUserInfo> lvUserInfo;
        public List<LvMonInfo> lvMonInfo;
        public List<Hero> hero;
        public List<Npc> npc;
        public List<Monster> monster;
        public List<MobItem> mobItem;
        public List<SkillProperty> skillProperty;
        public List<SkillData> skillData;
        public List<Item> item;
        public List<ItemEffectiveData> itemEffectiveData;
        public List<ItemBox> itemBox;
        public List<Production> production;
        public List<ItemPowerAdd> itemPowerAdd;
        public List<Npc_Sale> npcSale;
        public List<Zone> zone;
        public List<Warp> warp;
        public List<ItemRankInfo> itemRankInfo;
        public List<ItemTypeInfo> itemTypeInfo;
        public List<OPT_Info> optInfo;
        public List<OPT_Lv_Info> optLvInfo;
        public List<LootTypeInfo> lootTypeInfo;
        public List<LootRankInfo> lootRankInfo;
        public List<MixSkill> mixSkill;

        public SData()
        {
            lvUserInfo = new List<LvUserInfo>();
            lvMonInfo = new List<LvMonInfo>();
            hero = new List<Hero>();
            npc = new List<Npc>();
            monster = new List<Monster>();
            mobItem = new List<MobItem>();
            skillProperty = new List<SkillProperty>();
            skillData = new List<SkillData>();
            item = new List<Item>();
            itemEffectiveData = new List<ItemEffectiveData>();
            itemBox = new List<ItemBox>();
            production = new List<Production>();
            itemPowerAdd = new List<ItemPowerAdd>();
            npcSale = new List<Npc_Sale>();
            zone = new List<Zone>();
            warp = new List<Warp>();
            itemRankInfo = new List<ItemRankInfo>();
            itemTypeInfo = new List<ItemTypeInfo>();
            optInfo = new List<OPT_Info>();
            optLvInfo = new List<OPT_Lv_Info>();
            lootTypeInfo = new List<LootTypeInfo>();
            lootRankInfo = new List<LootRankInfo>();
            mixSkill = new List<MixSkill>();
        }




        public struct LvUserInfo
        {
            public short Lv;
            public double LvUpExp;
        }

        public struct LvMonInfo
        {
            public short Lv;
            public short HP;
            public short MP;
            public short STR;
            public short DEX;
            public short AIM;
            public short Luck;
            public short ATT;
            public short AP;
            public short DP;
            public short HitCnt;
            public short HitDice;
            public int GiveExp;
            public short MixRate;
        }

        public struct Hero
        {
            public byte type;
            public string name;
            public byte sex;
            public short birth_zone_idx;
            public short birth_zone_layernum;
            public short speed_move;
            public short speed_attack;
            public short speed_skill;
            public short base_str;
            public short base_dex;
            public short base_aim;
            public short base_luck;
            public short base_ap;
            public short base_dp;
            public short base_hc;
            public short base_hd;
            public short base_hp;
            public short base_mp;
            public short res_fire;
            public short res_water;
            public short res_earth;
            public short res_wind;
            public short res_devil;
            public short attr;
            public short make_freepoint;
            public short make_bonus_item0;
            public short make_bonus_item1;
            public short make_bonus_item2;
            public short skill_able;
            public int equip_able;
        }

        public struct Npc
        {
            public short idx;
            public string name;
            public byte type;
            public short birth_zone_idx;
            public byte birth_zone_x;
            public byte birth_zone_y;
            public short move_zone_layernum;
            public byte sell_type;
            public byte sell_ratio;
            public short barter_item_idx;
        }

        public struct Monster
        {
            public short type;
            public string name;
            public short race;
            public short start_base_level;
            public int price;
            public short sexratio;
            public short speed_move;
            public short speed_atack;
            public short hench_speed_attack;
            public short speed_skill;
            public short hench_speed_skill;
            public int core_rate;
            public float stat_rate;
            public float HenchStatRate;
            public short loot_type;
            public float hp_rate;
            public float hench_hp_rate;
            public float exp_rate;
            public short attack_range;
            public short hench_attack_range;
            public short restrict_type;
            public short sp;
            public short skill;
            public short attack_range_x1;
            public short attack_range_x2;
            public short attack_range_y1;
            public short attack_range_y2;
            public short use_item_type;
            public short mix_restrict;
            public int duration;
            public short duration_type;

        }

        public struct MobItem
        {
            public short id_idx;
            public int base_money;
            public int bonus_money;
            public short item_idx0;
            public short item_idx1;
            public short item_idx2;
            public short item_idx3;
            public short item_idx4;
            public short item_idx5;
            public short item_idx6;
            public short item_idx7;
            public short item_idx8;
            public short item_idx9;
            public int item_drop_percent0;
            public int item_drop_percent1;
            public int item_drop_percent2;
            public int item_drop_percent3;
            public int item_drop_percent4;
            public int item_drop_percent5;
            public int item_drop_percent6;
            public int item_drop_percent7;
            public int item_drop_percent8;
            public int item_drop_percent9;
            public short item_drop_count0;
            public short item_drop_count1;
            public short item_drop_count2;
            public short item_drop_count3;
            public short item_drop_count4;
            public short item_drop_count5;
            public short item_drop_count6;
            public short item_drop_count7;
            public short item_drop_count8;
            public short item_drop_count9;
        }

        public struct SkillProperty
        {
            public byte skillIndex;
            public string name;
            public string targetClass;
            public string pkTargetClass;
            public byte targetRangeClass;
            public byte positiveEffect;
            public byte effectIndex;
            public byte effectingStat;
            public byte maxLevel;
            public byte upgradeType;
            public byte requireUpdateType;
            public int learningGold;
            public short learningSP;

        }

        public struct SkillData
        {
            public byte skill_index;
            public byte level;
            public byte consumedMp;
            public byte maxTargetDistance;
            public byte targetRange;
            public short requireSP;
            public int continuityTime;
            public int coolTime;
        }

        public struct Item
        {
            public short idx;
            public string name;
            public int price;
            public short barter_price;
            public byte rarity;
            public byte type;
            public short maxCnt;
            public byte require_level;
            public byte require_type;
            public short require_value;
            public byte equip_type;
            public byte equip_part0;
            public byte equip_part1;
            public byte equip_part2;
            public byte block_part0;
            public byte block_part1;
            public short roll_spell_idx;
            public short roll_spell_level;
            public byte ech_type0;
            public byte ech_type1;
            public byte ech_type2;
            public byte ech_type3;
            public byte ech_type4;
            public byte ech_type5;
            public byte ech_type6;
            public byte ech_typenum0;
            public byte ech_typenum1;
            public byte ech_typenum2;
            public byte ech_typenum3;
            public byte ech_typenum4;
            public byte ech_typenum5;
            public byte ech_typenum6;
            public short ech_x0;
            public short ech_x1;
            public short ech_x2;
            public short ech_x3;
            public short ech_x4;
            public short ech_x5;
            public short ech_x6;
            public byte ech_speed_move;
            public short ech_speed_attack;
            public short ech_speed_skill;
            public byte range;
            public int duration;
            public byte kind;
            public byte rank;
            public byte duration_type;
            public short restrict_type;
            public byte make_synergy_type;
            public byte make_synergy_level;
        }

        public struct ItemEffectiveData
        {
            public short item_idx;
            public string name;
            public byte effective_type;
            public byte effective_sub_type;
            public int effective_value;
        }

        public struct ItemBox
        {
            public int idx;
            public int add_idx;
            public float rate;
            public int count;
        }

        public struct Production
        {
            public int idx;
            public int doc_idx;
            public string doc_name;
            public int result_idx;
            public string result_name;
            public int result_count;
            public int money;
            public int default_pro;
            public int add_pro;
            public int opt_slot_cnt;
            public int stuff_idx1;
            public string stuff_name1;
            public int stuff_count1;
            public int stuff_idx2;
            public string stuff_name2;
            public int stuff_count2;
            public int stuff_idx3;
            public string stuff_name3;
            public int stuff_count3;
            public int stuff_idx4;
            public string stuff_name4;
            public int stuff_count4;
            public int stuff_idx5;
            public string stuff_name5;
            public int stuff_count5;
        }

        public struct ItemPowerAdd
        {
            public int idx;
            public string set_name;
            public int helmet;
            public int armor;
            public int glove;
            public int boots;
            public int arm1;
            public int arm2;
            public int ring1;
            public int ring2;
            public int neck;
            public int effect;
            public int abi1;
            public int abi1_set;
            public int abi2;
            public int abi2_set;
        }

        public struct Npc_Sale
        {
            public short npc_idx;
            public byte sale_type;
            public int sale_idx;
            public short buy_ratio;
        }

        public struct Zone
        {
            public short idx;
            public string name;
            public byte mob_able;
            public byte revive_zone_layernum;
            public byte nonPkZoneLayernum;
            public byte dual_zone_layernum;
            public int min_mob;
            public int max_mob;
            public byte mob_peruser;
            public short min_level;
            public short max_level;
            public int restriction;
            public short item_idx;
            public byte ColisionLayer;
            public byte RootZone;
            public byte Ability;
            public float mob_damage_rate;
            public byte PkZoneFlag;
            public short dropitemidx;
            public byte dropItemCond;

        }

        public struct Warp
        {
            public short from_zone_idx;
            public short from_zone_attr;
            public short dest_zone_idx;
            public short dest_zone_layer;
        }

        public struct ItemRankInfo
        {
            public byte rank;
            public byte etc_broken_rate;
            public byte etc_up_rate;
            public byte etc_up_down_rate;
            public byte etc_up_broken_rate;
            public int g_rank_rate;
            public byte g_opt_rate;
        }

        public struct ItemTypeInfo
        {
            public byte item_type;
            public string type_name;
            public int gamble_money;
            public byte loot_rate;
        }

        public struct OPT_Info
        {
            public byte type;
            public byte loot_rate;
        }

        public struct OPT_Lv_Info
        {
            public byte opt_lv;
            public byte enchant_rate;
            public int gamble_rate;
        }

        public struct LootTypeInfo
        {
            public byte loot_type;
            public short sp_loot_rate;
            public short loot_rate;
            public int opt_rate1;
            public int opt_rate2;
        }

        public struct LootRankInfo
        {
            public byte loot_type;
            public short item_type;
            public byte item_rank;
            public int rank_rate;
            public byte opt1;
            public int opt1_rate1;
            public int opt1_rate2;
            public int opt1_rate3;
            public int opt1_rate4;
            public int opt1_rate5;
            public byte opt2;
            public int opt2_rate1;
            public int opt2_rate2;
            public int opt2_rate3;
            public int opt2_rate4;
            public int opt2_rate5;
            public byte opt3;
            public int opt3_rate1;
            public int opt3_rate2;
            public int opt3_rate3;
            public int opt3_rate4;
            public int opt3_rate5;
            public byte opt4;
            public int opt4_rate1;
            public int opt4_rate2;
            public int opt4_rate3;
            public int opt4_rate4;
            public int opt4_rate5;
            public byte opt5;
            public int opt5_rate1;
            public int opt5_rate2;
            public int opt5_rate3;
            public int opt5_rate4;
            public int opt5_rate5;
            public byte opt6;
            public int opt6_rate1;
            public int opt6_rate2;
            public int opt6_rate3;
            public int opt6_rate4;
            public int opt6_rate5;
            public byte opt7;
            public int opt7_rate1;
            public int opt7_rate2;
            public int opt7_rate3;
            public int opt7_rate4;
            public int opt7_rate5;
            public byte opt8;
            public int opt8_rate1;
            public int opt8_rate2;
            public int opt8_rate3;
            public int opt8_rate4;
            public int opt8_rate5;

        }

        public struct MixSkill
        {
            public byte MixSkillLelvel;
            public short StartHenchLevel;
            public short EndHenchLevel;
            public short MixSkillBasis;
            public short MixSkillStart;
            public short MixSkillMaster;
            public short MixSkillBonus;
            public byte MixSkillMaxRate;
        }



    }
}
