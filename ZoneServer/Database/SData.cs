using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace ZoneServer.Database
{
    public class SData
    {
        static string myConnectionString = "Persist Security Info=False;server=" + MyInfo.SYSDB_HOST + ";uid=" + MyInfo.SYSDB_USER + ";pwd=" + MyInfo.SYSDB_PASS + ";database=" + MyInfo.SYSDB_NAME + ";";
        static MySqlConnection conn;

        public static bool ConnectGamedata()
        {
            conn = new MySqlConnection(myConnectionString);
            try
            {
                conn.Open();
                LogManager.CLogManager.WriteConsoleLog("[I] Connect to S_Data DB Successfully...", ConsoleColor.Green);
                return true;
            }
            catch
            {
                return false;
            }
        }



        public static bool Load_LvUserInfo()
        {
            try
            {
                if(conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_lvuserinfo;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            Structs.Data.LvUserInfo temp = new Structs.Data.LvUserInfo();
                            temp.Lv = reader.GetInt16(0);
                            temp.LvUpExp = reader.GetInt64(1);

                            Data.SData.LvUserInfo.Add(temp);
                        }

                    }
                    else { reader.Close();  return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_LvMonInfo()
        {
            try
            {
                if(conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_lvmoninfo;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            Structs.Data.LvMonInfo Temp = new Structs.Data.LvMonInfo();
                            Temp.Lv = reader.GetInt16(0);
                            Temp.HP = reader.GetInt16(1);
                            Temp.MP = reader.GetInt16(2);
                            Temp.STR = reader.GetInt16(3);
                            Temp.DEX = reader.GetInt16(4);
                            Temp.AIM = reader.GetInt16(5);
                            Temp.Luck = reader.GetInt16(6);
                            Temp.ATT = reader.GetInt16(7);
                            Temp.AP = reader.GetInt16(8);
                            Temp.DP = reader.GetInt16(9);
                            Temp.HitCnt = reader.GetInt16(10);
                            Temp.HitDice = reader.GetInt16(11);
                            Temp.GiveExp = reader.GetInt32(12);
                            Temp.MixRate = reader.GetInt16(13);

                            Data.SData.LvMonInfo.Add(Temp);
                          
                           
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                } else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Hero()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_hero";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Hero temp = new Structs.Data.Hero();
                            temp.type = reader.GetByte(0);
                            temp.name = reader.GetString(1);
                            temp.sex = reader.GetByte(2);
                            temp.birth_zone_idx = reader.GetInt16(3);
                            temp.birth_zone_layernum = reader.GetInt16(4);
                            temp.speed_move = reader.GetInt16(5);
                            temp.speed_attack = reader.GetInt16(6);
                            temp.speed_skill = reader.GetInt16(7);
                            temp.base_str = reader.GetInt16(8);
                            temp.base_dex = reader.GetInt16(9);
                            temp.base_aim = reader.GetInt16(10);
                            temp.base_luck = reader.GetInt16(11);
                            temp.base_ap = reader.GetInt16(12);
                            temp.base_dp = reader.GetInt16(13);
                            temp.base_hc = reader.GetInt16(14);
                            temp.base_hd = reader.GetInt16(15);
                            temp.base_hp = reader.GetInt16(16);
                            temp.base_mp = reader.GetInt16(17);
                            temp.res_fire = reader.GetInt16(18);
                            temp.res_water = reader.GetInt16(19);
                            temp.res_earth = reader.GetInt16(20);
                            temp.res_wind = reader.GetInt16(21);
                            temp.res_devil = reader.GetInt16(22);
                            temp.attr = reader.GetInt16(23);
                            temp.make_freepoint = reader.GetInt16(24);
                            temp.make_bonus_item0 = reader.GetInt16(25);
                            temp.make_bonus_item1 = reader.GetInt16(26);
                            temp.make_bonus_item2 = reader.GetInt16(27);
                            temp.skill_able = reader.GetInt16(28);
                            temp.equip_able = reader.GetInt32(29);

                            Data.SData.Hero.Add(temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_NPC()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_npc;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Npc Temp = new Structs.Data.Npc();
                            Temp.idx = reader.GetInt16(0);
                            Temp.name = reader.GetString(1);
                            Temp.type = reader.GetByte(2);
                            Temp.birth_zone_idx = reader.GetInt16(3);
                            Temp.birth_zone_x = reader.GetByte(4);
                            Temp.birth_zone_y = reader.GetByte(5);
                            Temp.move_zone_layernum = reader.GetInt16(6);
                            Temp.sell_type = reader.GetByte(7);
                            Temp.sell_ratio = reader.GetByte(8);
                            Temp.barter_item_idx = reader.GetInt16(9);

                            Data.SData.Npc.Add(Temp);

                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Monster()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_monster ORDER BY type asc;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Monster Temp = new Structs.Data.Monster();
                            Temp.type = reader.GetInt16(0);
                            Temp.name = reader.GetString(1);
                            Temp.race = reader.GetInt16(2);
                            Temp.start_base_level = reader.GetInt16(3);
                            Temp.price = reader.GetInt32(4);
                            Temp.sexratio = reader.GetInt16(5);
                            Temp.speed_move = reader.GetInt16(6);
                            Temp.speed_atack = reader.GetInt16(7);
                            Temp.hench_speed_attack = reader.GetInt16(8);
                            Temp.speed_skill = reader.GetInt16(9);
                            Temp.hench_speed_skill = reader.GetInt16(10);
                            Temp.core_rate = reader.GetInt32(11);
                            Temp.stat_rate = reader.GetFloat(12);
                            Temp.HenchStatRate = reader.GetFloat(13);
                            Temp.loot_type = reader.GetInt16(14);
                            Temp.hp_rate = reader.GetFloat(15);
                            Temp.hench_hp_rate = reader.GetFloat(16);
                            Temp.exp_rate = reader.GetFloat(17);
                            Temp.attack_range = reader.GetInt16(18);
                            Temp.hench_attack_range = reader.GetInt16(19);
                            Temp.restrict_type = reader.GetInt16(20);
                            Temp.sp = reader.GetInt16(21);
                            Temp.skill = reader.GetInt16(22);
                            Temp.attack_range_x1 = reader.GetInt16(23);
                            Temp.attack_range_x2 = reader.GetInt16(24);
                            Temp.attack_range_y1 = reader.GetInt16(25);
                            Temp.attack_range_y2 = reader.GetInt16(26);
                            Temp.use_item_type = reader.GetInt16(27);
                            Temp.mix_restrict = reader.GetInt16(28);
                            Temp.duration = reader.GetInt32(29);
                            Temp.duration_type = reader.GetInt16(30);

                            Data.SData.Monster.Add(Temp);
                            
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool Load_MobItem()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_mobitem";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.MobItem Temp = new Structs.Data.MobItem();
                            Temp.id_idx = reader.GetInt16(0);
                            Temp.base_money = reader.GetInt32(1);
                            Temp.bonus_money = reader.GetInt32(2);
                            Temp.item_idx0 = reader.GetInt16(3);
                            Temp.item_idx1 = reader.GetInt16(4);
                            Temp.item_idx2 = reader.GetInt16(5);
                            Temp.item_idx3 = reader.GetInt16(6);
                            Temp.item_idx4 = reader.GetInt16(7);
                            Temp.item_idx5 = reader.GetInt16(8);
                            Temp.item_idx6 = reader.GetInt16(9);
                            Temp.item_idx7 = reader.GetInt16(10);
                            Temp.item_idx8 = reader.GetInt16(11);
                            Temp.item_idx9 = reader.GetInt16(12);
                            
                            Temp.item_drop_percent0 = reader.GetInt32(13);
                            Temp.item_drop_percent1 = reader.GetInt32(14);
                            Temp.item_drop_percent2 = reader.GetInt32(15);
                            Temp.item_drop_percent3 = reader.GetInt32(16);
                            Temp.item_drop_percent4 = reader.GetInt32(17);
                            Temp.item_drop_percent5 = reader.GetInt32(18);
                            Temp.item_drop_percent6 = reader.GetInt32(19);
                            Temp.item_drop_percent7 = reader.GetInt32(20);
                            Temp.item_drop_percent8 = reader.GetInt32(21);
                            Temp.item_drop_percent9 = reader.GetInt32(22);
                            Temp.item_drop_count0 = reader.GetInt16(23);
                            Temp.item_drop_count1 = reader.GetInt16(24);
                            Temp.item_drop_count2 = reader.GetInt16(25);
                            Temp.item_drop_count3 = reader.GetInt16(26);
                            Temp.item_drop_count4 = reader.GetInt16(27);
                            Temp.item_drop_count5 = reader.GetInt16(28);
                            Temp.item_drop_count6 = reader.GetInt16(29);
                            Temp.item_drop_count7 = reader.GetInt16(30);
                            Temp.item_drop_count8 = reader.GetInt16(31);
                            Temp.item_drop_count9 = reader.GetInt16(32);

                            Data.SData.MobItem.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_SkillProperty()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_skillproperty";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.SkillProperty Temp = new Structs.Data.SkillProperty();
                            Temp.skillIndex = reader.GetByte(0);
                            Temp.name = reader.GetString(1);
                            Temp.targetClass = reader.GetString(2);
                            Temp.pkTargetClass = reader.GetString(3);
                            Temp.targetRangeClass = reader.GetByte(4);
                            Temp.positiveEffect = reader.GetByte(5);
                            Temp.effectIndex = reader.GetByte(6);
                            Temp.effectingStat = reader.GetByte(7);
                            Temp.maxLevel = reader.GetByte(8);
                            Temp.upgradeType = reader.GetByte(9);
                            Temp.requireUpdateType = reader.GetByte(10);
                            Temp.learningGold = reader.GetInt32(11);
                            Temp.learningSP = reader.GetInt16(12);

                            Data.SData.SkillProperty.Add(Temp);
                           
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_SkillData()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_skilldata";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.SkillData Temp = new Structs.Data.SkillData();
                            Temp.skill_index = reader.GetByte(0);
                            Temp.level = reader.GetByte(1);
                            Temp.consumedMp = reader.GetByte(2);
                            Temp.maxTargetDistance = reader.GetByte(3);
                            Temp.targetRange = reader.GetByte(4);
                            Temp.requireSP = reader.GetInt16(5);
                            Temp.continuityTime = reader.GetInt32(6);
                            Temp.coolTime = reader.GetInt32(7);

                            Data.SData.SkillData.Add(Temp);
                           
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Item()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_item";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Item Temp = new Structs.Data.Item();
                            Temp.idx = reader.GetInt16(0);
                            Temp.name = reader.GetString(1);
                            Temp.price = reader.GetInt32(2);
                            Temp.barter_price = reader.GetInt16(3);
                            Temp.rarity = reader.GetByte(4);
                            Temp.type = reader.GetByte(5);
                            Temp.maxCnt = reader.GetInt16(6);
                            Temp.require_level = reader.GetByte(7);
                            Temp.require_type = reader.GetByte(8);
                            Temp.require_value = reader.GetInt16(9);
                            Temp.equip_type = reader.GetByte(10);
                            Temp.equip_part0 = reader.GetByte(11);
                            Temp.equip_part1 = reader.GetByte(12);
                            Temp.equip_part2 = reader.GetByte(13);
                            Temp.block_part0 = reader.GetByte(14);
                            Temp.block_part1 = reader.GetByte(15);
                            Temp.roll_spell_idx = reader.GetInt16(16);
                            Temp.roll_spell_level = reader.GetInt16(17);
                            Temp.ech_type0 = reader.GetByte(18);
                            Temp.ech_type1 = reader.GetByte(19);
                            Temp.ech_type2 = reader.GetByte(20);
                            Temp.ech_type3 = reader.GetByte(21);
                            Temp.ech_type4 = reader.GetByte(22);
                            Temp.ech_type5 = reader.GetByte(23);
                            Temp.ech_type6 = reader.GetByte(24);
                            Temp.ech_typenum0 = reader.GetByte(25);
                            Temp.ech_typenum1 = reader.GetByte(26);
                            Temp.ech_typenum2 = reader.GetByte(27);
                            Temp.ech_typenum3 = reader.GetByte(28);
                            Temp.ech_typenum4 = reader.GetByte(29);
                            Temp.ech_typenum5 = reader.GetByte(30);
                            Temp.ech_typenum6 = reader.GetByte(31);
                            Temp.ech_x0 = reader.GetInt16(32);
                            Temp.ech_x1 = reader.GetInt16(33);
                            Temp.ech_x2 = reader.GetInt16(34);
                            Temp.ech_x3 = reader.GetInt16(35);
                            Temp.ech_x4 = reader.GetInt16(36);
                            Temp.ech_x5 = reader.GetInt16(37);
                            Temp.ech_x6 = reader.GetInt16(38);
                            Temp.ech_speed_move = reader.GetByte(39);
                            Temp.ech_speed_attack = reader.GetInt16(40);
                            Temp.ech_speed_skill = reader.GetInt16(41);
                            Temp.range = reader.GetByte(42);
                            Temp.duration = reader.GetInt32(43);
                            Temp.kind = reader.GetByte(44);
                            Temp.rank = reader.GetByte(45);
                            Temp.duration_type = reader.GetByte(46);
                            Temp.restrict_type = reader.GetInt16(47);
                            Temp.make_synergy_type = reader.GetByte(48);
                            Temp.make_synergy_level = reader.GetByte(49);

                            Data.SData.Item.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_ItemEffectiveData()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_itemeffectivedata";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.ItemEffectiveData Temp = new Structs.Data.ItemEffectiveData();
                            Temp.item_idx = reader.GetInt16(0);
                            Temp.name = reader.GetString(1);
                            Temp.effective_type = reader.GetByte(2);
                            Temp.effective_sub_type = reader.GetByte(3);
                            Temp.effective_value = reader.GetInt32(4);

                            Data.SData.ItemEffectiveData.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_ItemBox()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_itembox";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.ItemBox Temp = new Structs.Data.ItemBox();
                            Temp.idx = reader.GetInt32(0);
                            Temp.add_idx = reader.GetInt32(1);
                            Temp.rate = reader.GetFloat(2);
                            Temp.count = reader.GetInt32(3);

                            Data.SData.ItemBox.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Production()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_production;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Production Temp = new Structs.Data.Production();
                            Temp.idx = reader.GetInt32(0);
                            Temp.doc_idx = reader.GetInt32(1);
                            Temp.doc_name = reader.GetString(2);
                            Temp.result_idx = reader.GetInt32(3);
                            Temp.result_name = reader.GetString(4);
                            Temp.result_count = reader.GetInt32(5);
                            Temp.money = reader.GetInt32(6);
                            Temp.default_pro = reader.GetInt32(7);
                            Temp.add_pro = reader.GetInt32(8);
                            Temp.opt_slot_cnt = reader.GetInt32(9);

                            Temp.stuff_idx1 = reader.GetInt32(10);
                            Temp.stuff_name1 = reader.GetString(11);
                            Temp.stuff_count1 = reader.GetInt32(12);

                            Temp.stuff_idx2 = reader.GetInt32(13);
                            Temp.stuff_name2 = reader.GetString(14);
                            Temp.stuff_count2 = reader.GetInt32(15);

                            Temp.stuff_idx3 = reader.GetInt32(16);
                            Temp.stuff_name3 = reader.GetString(17);
                            Temp.stuff_count3 = reader.GetInt32(18);

                            Temp.stuff_idx4 = reader.GetInt32(19);
                            Temp.stuff_name4 = reader.GetString(20);
                            Temp.stuff_count4 = reader.GetInt32(21);

                            Temp.stuff_idx5 = reader.GetInt32(22);
                            Temp.stuff_name5 = reader.GetString(23);
                            Temp.stuff_count5 = reader.GetInt32(24);

                            Data.SData.Production.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_ItemPowerAdd()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_itempoweradd;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.ItemPowerAdd Temp = new Structs.Data.ItemPowerAdd();
                            Temp.idx = reader.GetInt32(0);
                            Temp.set_name = reader.GetString(1);
                            Temp.helmet = reader.GetInt32(2);
                            Temp.armor = reader.GetInt32(3);
                            Temp.glove = reader.GetInt32(4);
                            Temp.boots = reader.GetInt32(5);
                            Temp.arm1 = reader.GetInt32(6);
                            Temp.arm2 = reader.GetInt32(7);
                            Temp.ring1 = reader.GetInt32(8);
                            Temp.ring2 = reader.GetInt32(9);
                            Temp.neck = reader.GetInt32(10);
                            Temp.effect = reader.GetInt32(11);
                            Temp.abi1 = reader.GetInt32(12);
                            Temp.abi1_set = reader.GetInt32(13);
                            Temp.abi2 = reader.GetInt32(14);
                            Temp.abi2_set = reader.GetInt32(15);

                            Data.SData.ItemPowerAdd.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_NpcSale()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_npc_sale";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Npc_Sale Temp = new Structs.Data.Npc_Sale();
                            Temp.npc_idx = reader.GetInt16(0);
                            Temp.sale_type = reader.GetByte(1);
                            Temp.sale_idx = reader.GetInt32(2);
                            Temp.buy_ratio = reader.GetInt16(3);

                            Data.SData.NPC_Sale.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Zone()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_zone;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Zone Temp = new Structs.Data.Zone();
                            Temp.idx = reader.GetInt16(0);
                            Temp.name = reader.GetString(1);
                            Temp.mob_able = reader.GetByte(2);
                            Temp.revive_zone_layernum = reader.GetByte(3);
                            Temp.nonPkZoneLayernum = reader.GetByte(4);
                            Temp.dual_zone_layernum = reader.GetByte(5);
                            
                            Temp.min_mob = reader.GetInt32(6);
                            Temp.max_mob = reader.GetInt32(7);
                            
                            Temp.mob_peruser = reader.GetByte(8);
                            Temp.min_level = reader.GetInt16(9);
                            Temp.max_level = reader.GetInt16(10);
                            Temp.restriction = reader.GetInt32(11);
                            Temp.item_idx = reader.GetInt16(12);
                            Temp.ColisionLayer = reader.GetByte(13);
                            Temp.RootZone = reader.GetByte(14);
                            Temp.Ability = reader.GetByte(15);
                            Temp.mob_damage_rate = reader.GetFloat(16);
                            Temp.PkZoneFlag = reader.GetByte(17);
                            Temp.dropitemidx = reader.GetInt16(18);
                            Temp.dropItemCond = reader.GetByte(19);

                            Data.SData.Zone.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_Gate()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_gate;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.Warp Temp = new Structs.Data.Warp();
                            Temp.from_zone_idx = reader.GetInt16(0);
                            Temp.from_zone_attr = reader.GetInt16(1);
                            Temp.dest_zone_idx = reader.GetInt16(2);
                            Temp.dest_zone_layer = reader.GetInt16(3);

                            Data.SData.Warp.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_ItemRankInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_ItemTypeInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_OPTInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_OPTLvInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_LootTypeInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_LootRankInfo()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Write("");
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Load_MixSkill()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from s_mixskill";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Data.MixSkill Temp = new Structs.Data.MixSkill();
                            Temp.MixSkillLelvel = reader.GetByte(0);
                            Temp.StartHenchLevel = reader.GetInt16(1);
                            Temp.EndHenchLevel = reader.GetInt16(2);
                            Temp.MixSkillBasis = reader.GetInt16(3);
                            Temp.MixSkillStart = reader.GetInt16(4);
                            Temp.MixSkillMaster = reader.GetInt16(5);
                            Temp.MixSkillBonus = reader.GetInt16(6);
                            Temp.MixSkillMaxRate = reader.GetByte(7);

                            Data.SData.MixSkill.Add(Temp);
                        }

                    }
                    else { reader.Close(); return false; }
                    reader.Close();
                }
                else { return false; }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
