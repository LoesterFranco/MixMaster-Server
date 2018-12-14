using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using ZoneServer.Network.ZS;


namespace ZoneServer.Database
{
    public class Gamedata
    {
        static string myConnectionString = "Persist Security Info=False;server=" + MyInfo.GAMEDB_HOST + ";uid=" + MyInfo.GAMEDB_USER + ";pwd=" + MyInfo.GAMEDB_PASS + ";database=" + MyInfo.GAMEDB_NAME + ";";
        static MySqlConnection conn;

        public static bool ConnectGamedata()
        {
            conn = new MySqlConnection(myConnectionString);
            try
            {
                conn.Open();
                LogManager.CLogManager.WriteConsoleLog("[I] Connect to Gamedata DB Successfully...", ConsoleColor.Green);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static List<Structs.gamedata.QuestLog> GetQuestsLog(Client hero)
        {
            List<Structs.gamedata.QuestLog> quest = new List<Structs.gamedata.QuestLog>();

            try
            {
                if(conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_QuestLog WHERE id_idx = @id and hero_order = @order;";
                    cmd.Parameters.AddWithValue("@id", hero.data.Hero.id_idx);
                    cmd.Parameters.AddWithValue("@order", hero.data.Hero.hero_order);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            Structs.gamedata.QuestLog temp = new Structs.gamedata.QuestLog();
                            temp.quest_index = reader.GetInt16(2);
                            temp.quest_state = reader.GetByte(3);
                            quest.Add(temp);
                        }
                    } else { reader.Close(); }
                    reader.Close();

                }
            }
            catch
            {

            }
            return quest;
        }


        public static List<Structs.gamedata.u_hench> GetHenchs(Client Hero)
        {
            List<Structs.gamedata.u_hench> Henchs = new List<Structs.gamedata.u_hench>();
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM u_hench_0 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_1 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_2 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_3 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_4 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_5 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_6 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_7 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_8 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_9 WHERE id_idx = @ID AND hero_order = @HERO_ORDER order by hench_order asc;";
                    cmd.Parameters.AddWithValue("@ID", Hero.data.Hero.id_idx);
                    cmd.Parameters.AddWithValue("@HERO_ORDER", Hero.data.Hero.hero_order);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.gamedata.u_hench hench = new Structs.gamedata.u_hench();
                            hench.position = reader.GetByte(3);
                            hench.hench_order = reader.GetByte(4);
                            hench.HenchID = reader.GetInt16(5);
                            hench.HenchName = reader.GetString(6);
                            hench.sex = reader.GetByte(7);
                            hench.IsDead = reader.GetBoolean(8);
                            hench.mixnum = (byte)reader.GetInt16(9);
                            hench.hench_lv = (byte)reader.GetInt16(10);
                            hench.hench_lvmax = (byte)reader.GetInt16(11);
                            hench.hench_exp = reader.GetInt64(12);
                            long back = (long)Data.SData.LvUserInfo[hench.hench_lv].LvUpExp; // back lv
                            long next = (long)Data.SData.LvUserInfo[hench.hench_lv + 1].LvUpExp; // next lv
                            hench.hench_exp_backlevel = back;
                            hench.hench_exp_nextlevel = next;

                            hench.str = reader.GetInt16(16);
                            hench.dex = reader.GetInt16(17);
                            hench.aim = reader.GetInt16(18);
                            hench.luck = reader.GetInt16(19);
                            hench.ap = reader.GetInt16(20);
                            hench.dp = reader.GetInt16(21);
                            hench.hc = reader.GetInt16(22);
                            hench.hd = reader.GetInt16(23);
                            hench.hp = reader.GetInt16(24);
                            hench.mp = reader.GetInt16(25);
                            hench.max_hp = (short)(hench.hench_lv * 30); // example
                            hench.max_mp = (short)(hench.hench_lv * 15); // example
                            hench.growthtype = (byte)reader.GetInt16(31);
                            hench.race_val = (byte)reader.GetInt16(32);
                            hench.enchant_grade = (byte)reader.GetInt16(35);
                            hench.item_slot_total = (byte)reader.GetInt16(36);
                            hench.item0_idx = reader.GetInt16(37);
                            hench.item0_duration = reader.GetInt32(39);
                            hench.item1_idx = reader.GetInt16(40);
                            hench.item1_duration = reader.GetInt32(42);
                            hench.item2_idx = reader.GetInt16(43);
                            hench.item2_duration = (int)reader.GetInt32(45);
                            hench.duration = reader.GetInt32(46);
                            Henchs.Add(hench);
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();

                }
            }
            catch(Exception er)
            {
                Console.WriteLine("Error: " + er.ToString());
            }
            return Henchs;
        }

        public static bool LoadGuilds()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_guild;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Guild.guild Temp = new Structs.Guild.guild();
                            Temp.GuildIdx = reader.GetInt16(0);
                            Temp.Name = reader.GetString(1);
                            Temp.Info = reader.GetString(2);
                            Temp.Cert = reader.GetString(3);
                            Temp.EstablishDate = reader.GetDateTime(4);
                            Temp.LimitCount = reader.GetInt16(5);
                            Temp.Status = reader.GetByte(6);
                            Temp.MarkRegDate = reader.GetDateTime(7);
                            Temp.MarkRegCnt = reader.GetByte(8);
                            Temp.gold = reader.GetInt32(10);
                            Temp.HiringIdx = reader.GetInt16(11);
                            Temp.CertDate = reader.GetDateTime(12);
                            Temp.InfoDate = reader.GetDateTime(13);

                            Data.Guild.Guilds.Add(Temp);

                        }

                    }
                    else { reader.Close(); return false; }

                    reader.Close();

                    if (LoadGuildMember())
                    {
                        return true;
                    } else { return false; }
                    
                }
                else { return false; }

            }
            catch
            {
                return false;
            }
        }


        public static bool LoadGuildMember()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_guildmember;";

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Structs.Guild.guild_member Temp = new Structs.Guild.guild_member();
                            Temp.HeroIdx = reader.GetInt32(0);
                            Temp.HeroOrder = reader.GetByte(1);
                            Temp.GuildIdx = reader.GetInt16(2);
                            Temp.MemberID = reader.GetInt16(3);
                            Temp.Grade = reader.GetInt16(4);
                            Temp.Authority = reader.GetInt32(5);
                            Temp.Memo = reader.GetString(6);

                            Data.Guild.Guild_Members.Add(Temp);

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



        public static XHERO LoadHero(int id_idx, byte hero_order)
        {
            XHERO temp = new XHERO();
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_hero WHERE id_idx = @ID AND hero_order = @ORDER;";
                    cmd.Parameters.AddWithValue("@ID", id_idx);
                    cmd.Parameters.AddWithValue("@ORDER", hero_order);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            temp.id_idx = reader.GetInt32(0);
                            temp.hero_order = reader.GetByte(1);
                            temp.serial = reader.GetInt64(2);
                            temp.Class = reader.GetByte(3);
                            temp.name = reader.GetString(4);
                            temp.hero_type = reader.GetByte(5);
                            temp.now_zone_idx = reader.GetInt16(6);
                            temp.now_zone_x = reader.GetByte(7);
                            temp.now_zone_y = reader.GetByte(8);
                            temp.init_pos_layer = reader.GetInt16(9);
                            temp.revive_zone_idx = reader.GetInt16(10);
                            temp.baselevel = reader.GetInt16(11);
                            temp.gold = reader.GetInt32(12);
                            temp.attr = reader.GetInt32(13);
                            temp.exp = reader.GetInt64(14);
                            temp.speed_move = reader.GetByte(15);
                            temp.speed_attack = reader.GetInt16(16);
                            temp.speed_skill = reader.GetInt16(17);
                            temp.str = reader.GetInt32(18);
                            temp.dex = reader.GetInt32(19);
                            temp.aim = reader.GetInt32(20);
                            temp.luck = reader.GetInt32(21);
                            temp.ap = reader.GetInt16(22);
                            temp.dp = reader.GetInt16(23);
                            temp.hc = reader.GetInt16(24);
                            temp.hd = reader.GetInt16(25);
                            temp.hp = reader.GetInt32(26);
                            temp.mp = reader.GetInt32(27);
                            temp.maxhp = reader.GetInt32(28);
                            temp.maxmp = reader.GetInt32(29);
                            temp.abil_freepoint = reader.GetInt32(30);
                            temp.res_fire = reader.GetInt16(31);
                            temp.res_water = reader.GetInt16(32);
                            temp.res_earth = reader.GetInt16(33);
                            temp.res_wind = reader.GetInt16(34);
                            temp.res_devil = reader.GetInt16(35);
                            temp.ign_att_cnt = reader.GetByte(36);
                            temp.regdate = reader.GetDateTime(37);
                            temp.avatar_head = reader.GetInt16(38);
                            temp.avatar_body = reader.GetInt16(39);
                            temp.avatar_foot = reader.GetInt16(40);
                            temp.return_time = reader.GetInt32(41);
                            temp.status = reader.GetByte(42);
                            temp.status_time = reader.GetDateTime(43);
                            temp.nickname = reader.GetInt16(44);
                            temp.last_logout_time = reader.GetDateTime(45);
                            temp.skill_point = reader.GetInt32(46);
                            temp.login = reader.GetByte(47);
                        }
                    }
                    Console.WriteLine("Sucesso!");
                    reader.Close();
                }
            }
            catch
            {

            }

            return temp;
        }
    }
}
