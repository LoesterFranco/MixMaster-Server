using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using ZoneServer.GameServerManager.Data;
using ZoneServer.GameServerManager;
using System.Data;
using ZoneServer.Network;

namespace ZoneServer.Database
{

    public class DGameData
    {
        private string HOST = "";
        private string USER = "";
        private string PASS = "";
        private string DB_NAME = "";
        private string connectionString = "";
        private MySqlConnection conn;

        public DGameData(string host, string user, string pass, string dbName)
        {
            HOST = host;
            USER = user;
            PASS = pass;
            DB_NAME = dbName;
            connectionString = "Persist Security Info=False;server=" + this.HOST + ";uid=" + this.USER + ";pwd=" + this.PASS + ";database=" + this.DB_NAME + ";";
            conn = new MySqlConnection(connectionString);
        }

        public bool Connect()
        {
            if (conn.State == System.Data.ConnectionState.Open) return true;
            try
            {
                conn.Open();
                Init.logger.ConsoleLog($"Database: {DB_NAME} conectado com sucesso!", ConsoleColor.Green);
                Init.logger.WriteLog($"Database: {DB_NAME} conectado com sucesso!",LogStatus.DatabaseInfo);
                return true;
            }
            catch(Exception e)
            {
                Init.logger.ConsoleLog($"Ocorreu um erro na conexão da Database: {this.DB_NAME}.", ConsoleColor.Red);
                Init.logger.WriteLog($"Ocorreu um erro na conexão da Database: {this.DB_NAME}. Erro: " + e.Message, LogStatus.DatabaseError);
                return false;
            }
        }

        public void LoadAll()
        {
            if(!LoadGuilds())
            {
                Init.logger.WriteLog($"Database: {DB_NAME} ERROR: Falha ao carregar u_guild", LogStatus.DatabaseError);
                return;
            }
            Init.logger.WriteLog("Table: u_guild carregado com sucesso!", LogStatus.DatabaseInfo);
            Init.logger.ConsoleLog((char)0x09 + "[Database] u_guild carregado com sucesso!", ConsoleColor.Yellow);

            if(!LoadGuildMembers())
            {
                Init.logger.WriteLog($"Database: {DB_NAME} ERROR: Falha ao carregar u_guildmember", LogStatus.DatabaseError);
                return;
            }
            Init.logger.WriteLog($"Table: u_guildmember carregado com sucesso!", LogStatus.DatabaseInfo);
            Init.logger.ConsoleLog((char)0x09 + "[Database] u_guildmember carregado com sucesso!", ConsoleColor.Yellow);


        }

        private bool LoadGuilds()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) return false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from u_guild;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows) { reader.Close(); return false; }

                while(reader.Read())
                {
                    GameData.XGuild Temp = new GameData.XGuild();
                    Temp.GuildIdx = reader.GetUInt16(0);
                    Temp.Name = reader.GetString(1);
                    Temp.Info = reader.GetString(2);
                    Temp.Cert = reader.GetString(3);
                    Temp.EstablishDate = reader.GetDateTime(4);
                    Temp.LimitCount = reader.GetUInt16(5);
                    Temp.Status = reader.GetByte(6);
                    Temp.MarkRegDate = reader.GetDateTime(7);
                    Temp.MarkRegCnt = reader.GetByte(8);
                    Temp.gold = reader.GetUInt32(10);
                    Temp.HiringIdx = reader.GetUInt16(11);
                    Temp.CertDate = reader.GetDateTime(12);
                    Temp.InfoDate = reader.GetDateTime(13);

                    Init.game.gamedata.guild.Add(Temp);
                }
                reader.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }
        private bool LoadGuildMembers()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) return false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from u_guildmember;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows) { reader.Close(); return false; }

                while (reader.Read())
                {
                    GameData.XGuildMember Temp = new GameData.XGuildMember();
                    Temp.HeroIdx = reader.GetUInt32(0);
                    Temp.HeroOrder = reader.GetByte(1);
                    Temp.GuildIdx = reader.GetUInt16(2);
                    Temp.MemberID = reader.GetUInt16(3);
                    Temp.Grade = reader.GetUInt16(4);
                    Temp.Authority = reader.GetUInt32(5);
                    Temp.Memo = reader.GetString(6);
                    Init.game.gamedata.guildMember.Add(Temp);
                }
                reader.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public void LoadMyHero(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("[DATABASE] Pegando dados do personagem!");
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_hero WHERE id_idx = @ID AND hero_order = @ORDER;";
                    cmd.Parameters.AddWithValue("@ID", hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@ORDER", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            hero.id_idx = reader.GetUInt32(0);
                            hero.hero_order = reader.GetByte(1);
                            hero.serial = reader.GetInt64(2);
                            hero.Class = reader.GetByte(3);
                            hero.name = reader.GetString(4);
                            hero.hero_type = reader.GetByte(5);
                            hero.now_zone_idx = reader.GetUInt16(6);
                            hero.now_zone_x = reader.GetByte(7);
                            hero.now_zone_y = reader.GetByte(8);
                            hero.init_pos_layer = reader.GetUInt16(9);
                            hero.revive_zone_idx = reader.GetUInt16(10);
                            hero.baselevel = reader.GetUInt16(11);
                            hero.gold = reader.GetUInt32(12);
                            hero.attr = reader.GetUInt32(13);
                            hero.exp = reader.GetInt64(14);
                            hero.speed_move = reader.GetByte(15);
                            hero.speed_attack = reader.GetUInt16(16);
                            hero.speed_skill = reader.GetUInt16(17);
                            hero.str = reader.GetUInt32(18);
                            hero.dex = reader.GetUInt32(19);
                            hero.aim = reader.GetUInt32(20);
                            hero.luck = reader.GetUInt32(21);
                            hero.ap = reader.GetUInt16(22);
                            hero.dp = reader.GetUInt16(23);
                            hero.hc = reader.GetUInt16(24);
                            hero.hd = reader.GetUInt16(25);
                            hero.hp = reader.GetUInt32(26);
                            hero.mp = reader.GetUInt32(27);
                            hero.maxhp = reader.GetUInt32(28);
                            hero.maxmp = reader.GetUInt32(29);
                            hero.abil_freepoint = reader.GetUInt32(30);
                            hero.res_fire = reader.GetUInt16(31);
                            hero.res_water = reader.GetUInt16(32);
                            hero.res_earth = reader.GetUInt16(33);
                            hero.res_wind = reader.GetUInt16(34);
                            hero.res_devil = reader.GetUInt16(35);
                            hero.ign_att_cnt = reader.GetByte(36);
                            hero.regdate = reader.GetDateTime(37);
                            hero.avatar_head = reader.GetUInt16(38);
                            hero.avatar_body = reader.GetUInt16(39);
                            hero.avatar_foot = reader.GetUInt16(40);
                            hero.return_time = reader.GetUInt32(41);
                            hero.status = reader.GetByte(42);
                            hero.status_time = reader.GetDateTime(43);
                            hero.nickname = reader.GetUInt16(44);
                            hero.last_logout_time = reader.GetDateTime(45);
                            hero.skill_point = reader.GetUInt32(46);
                            hero.login = reader.GetByte(47);
                        }
                    }
                    reader.Close();
                }
            }
            catch
            {

            }
        }
        public void LoadMyHenchs(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM u_hench_0 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_1 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_2 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_3 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_4 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_5 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_6 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_7 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_8 WHERE id_idx = @ID AND hero_order = @HERO_ORDER UNION SELECT * FROM u_hench_9 WHERE id_idx = @ID AND hero_order = @HERO_ORDER order by hench_order asc;";
                    cmd.Parameters.AddWithValue("@ID", hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@HERO_ORDER", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            XHENCH_INFO hench = new XHENCH_INFO(hero);
                            hench.position = reader.GetByte(3);
                            hench.hench_order = reader.GetByte(4);
                            hench.HenchID = reader.GetUInt16(5);
                            hench.HenchName = reader.GetString(6);
                            hench.sex = reader.GetByte(7);
                            hench.IsDead = reader.GetBoolean(8);
                            hench.mixnum = (byte)reader.GetInt16(9);
                            hench.hench_lv = reader.GetUInt16(10);
                            hench.hench_lvmax = reader.GetUInt16(11);
                            hench.hench_exp = reader.GetInt64(12);

                            long back = (long)Init.game.sdata.lvUserInfo[hench.hench_lv].LvUpExp; // back lv
                            long next = 500;
                            hench.hench_exp_backlevel = back;
                            hench.hench_exp_nextlevel = next;

                            hench.str = reader.GetUInt16(16);
                            hench.dex = reader.GetUInt16(17);
                            hench.aim = reader.GetUInt16(18);
                            hench.luck = reader.GetUInt16(19);
                            hench.ap = reader.GetUInt16(20);
                            hench.dp = reader.GetUInt16(21);
                            hench.hc = reader.GetUInt16(22);
                            hench.hd = reader.GetUInt16(23);
                            hench.hp = reader.GetUInt32(24);
                            hench.mp = reader.GetUInt32(25);
                            hench.max_hp = 65535; // example
                            hench.max_mp = 65535; // example
                            hench.growthtype = (byte)reader.GetInt16(31);
                            hench.race_val = reader.GetUInt16(32);
                            hench.enchant_grade = (byte)reader.GetInt16(35);
                            hench.item_slot_total = (byte)reader.GetInt16(36);
                            hench.item0_idx = reader.GetUInt16(37);
                            hench.item0_duration = reader.GetUInt32(39);
                            hench.item1_idx = reader.GetUInt16(40);
                            hench.item1_duration = reader.GetUInt32(42);
                            hench.item2_idx = reader.GetUInt16(43);
                            hench.item2_duration = reader.GetUInt32(45);
                            hench.duration = reader.GetInt32(46);


                            if (hench.position == 0)
                                hero.AddBattleHench(new XBATTLE_HENCH(hench));
                            else
                                hero.AddHench(hench);
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hench load filed!" + e.Message);
            }
        }
        public void LoadMyQuests(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_QuestLog WHERE id_idx = @id and hero_order = @order;";
                    cmd.Parameters.AddWithValue("@id", hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@order", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CQuest quest = new CQuest(hero);

                            quest.quest_index = reader.GetUInt16(2);
                            quest.quest_state = reader.GetByte(3);

                            hero.AddQuest(quest);
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();

                }
            }
            catch
            {
                return;
            }
        }
        public void LoadMySkills(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_heroskill WHERE heroIndex = @id and heroSocketNum = @order;";
                    cmd.Parameters.AddWithValue("@id", hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@order", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CSkill skill = new CSkill(hero);
                            skill.skillID = reader.GetByte(2);
                            skill.skillLevel = reader.GetByte(3);
                            skill.skillPoint = reader.GetUInt16(4);
                            skill.learningDate = reader.GetDateTime(5);
                            hero.AddSkill(skill);
                            Console.WriteLine("skill adicionada!");
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();

                }
            }
            catch
            {

            }
        }
        public void LoadMixSkill(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_MixSkill WHERE HeroIdx = @id and HeroOrder = @order;";
                    cmd.Parameters.AddWithValue("@id", hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@order", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            hero.mixSkill.MixSkill1 = reader.GetUInt16(2);
                            hero.mixSkill.MixSkill2 = reader.GetUInt16(3);
                            hero.mixSkill.MixSkill3 = reader.GetUInt16(4);
                            hero.mixSkill.MixSkill4 = reader.GetUInt16(5);
                            hero.mixSkill.MixSkill5 = reader.GetUInt16(6);
                            hero.mixSkill.MixSkill6 = reader.GetUInt16(7);
                            hero.mixSkill.MixSkill7 = reader.GetUInt16(8);
                            hero.mixSkill.MixSkill8 = reader.GetUInt16(9);
                            hero.mixSkill.MixSkill9 = reader.GetUInt16(10);
                            hero.mixSkill.MixSkill10 = reader.GetUInt16(11);

                            Console.WriteLine("MixSkill setado!");
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();

                }
            }
            catch
            {

            }
        }
        public void LoadMyItems(XHERO hero)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from u_item WHERE id_idx = @id and hero_order = @order;";
                    cmd.Parameters.AddWithValue("@id",  hero.zs_data.ID_IDX);
                    cmd.Parameters.AddWithValue("@order", hero.zs_data.HERO_ORDER);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CItem item = new CItem(hero);

                            item.id = reader.GetUInt32(3);
                            item.socket_type = reader.GetByte(4);
                            item.socket_num = reader.GetByte(5);
                            item.count = reader.GetUInt16(6);
                            item.opt = reader.GetByte(7);
                            item.opt_level = reader.GetByte(8);
                            item.duration = reader.GetUInt32(9);
                            item.last_check_time = reader.GetDateTime(10);
                            item.synergy = reader.GetByte(11);
                            item.synergy_level = reader.GetByte(12);


                            hero.AddItem(item);
                            Console.WriteLine("item adicionado!");
                        }
                    }
                    else { reader.Close(); }
                    reader.Close();
                }
            }
            catch
            {

            }
        }

    }
}
