using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Common;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;
using Game_Manager_Server.MixMaster_API.Network;

namespace Game_Manager_Server.MixMaster_API.Database
{
    public class gamedata
    {
        static string myConnectionString = "Persist Security Info=False;server=" + MyInfo.GAMEDB_HOST + ";uid=" + MyInfo.GAMEDB_USER + ";pwd="+MyInfo.GAMEDB_PASS+";database="+MyInfo.GAMEDB_NAME+";";
        static MySqlConnection conn;


        public static bool ConnectGamedata()
        {
            conn = new MySqlConnection(myConnectionString);
            try
            {
                conn.Open();
                Console.WriteLine("[I] Connect to gamedata DB Successfully...");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static String GetTimestamp(DateTime value) {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        public static bool PutDeleteCharDate(int id_idx, byte hero_order)
        {
            bool result = false;
            if (conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE u_hero SET status = @STATUS, status_time = @STATUS_TIME WHERE id_idx = @ID AND hero_order = @HERO_ORDER";
                cmd.Parameters.AddWithValue("@ID", id_idx);
                cmd.Parameters.AddWithValue("@HERO_ORDER", hero_order);
                cmd.Parameters.AddWithValue("@STATUS", 1);
                DateTime localDate = DateTime.Now;
                TimeSpan duration = new TimeSpan(7,0,0,0,0); // 7 days
                DateTime status_time = localDate.Add(duration); // adiciona uma semana de prazo para a exclusão do personagem
                string time = status_time.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@STATUS_TIME", time);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.RecordsAffected > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                reader.Close();
            }
            return result;
        }


        public static bool CharacterNameExists(string username)
        {
            bool result = true;

            if (conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM u_hero WHERE name = '"+username+"';";

                MySqlDataReader datareader = cmd.ExecuteReader();
                if (datareader.HasRows)
                {
                    datareader.Close();
                    result = true;
                    //Console.WriteLine("character existe!");
                }
                else
                {
                    datareader.Close();
                    result = false;
                   // Console.WriteLine("character não existe!");
                }
            }

            return result;
        }


        public static bool CreateCharacter(ClientManager MyClient, XHERO MyHero)
        {
            bool result = false;

            if (conn.State == ConnectionState.Open)
            {
                String timeStamp = GetTimestamp(DateTime.Now);
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO u_hero(id_idx, hero_order, serial, name, hero_type, now_zone_idx, init_pos_layer, revive_zone_idx, baselevel, speed_move, str, dex, aim, luck, res_fire, res_water, res_earth, res_wind, res_devil, hc, hd, ap, dp, hp, mp, regdate, avatar_head, last_logout_time, skill_point)VALUES(@ID, @HERO_ORDER, @SERIAL, @NAME, @HERO_TYPE, @NOW_ZONE_IDX, @INIT_POS_LAYER, @REVIVE_ZONE_IDX, @BASELEVEL, @SPEEDMOVE, @STR, @DEX, @AIM, @LUCK, @RES_FIRE, @RES_WATER, @RES_EARTH, @RES_WIND, @RES_DEVIL, @HC, @HD, @AP, @DP, @HP, @MP, @REGDATE, @AVATAR_HEAD, @LAST_LOGOUT_TIME, @SKILL_POINT);";
                cmd.Parameters.AddWithValue("@ID", MyClient.data.id_idx);
                cmd.Parameters.AddWithValue("@HERO_ORDER", MyHero.hero_order);
                cmd.Parameters.AddWithValue("@SERIAL", timeStamp);
                cmd.Parameters.AddWithValue("@NAME", MyHero.name);
                cmd.Parameters.AddWithValue("@HERO_TYPE", MyHero.hero_type);
                cmd.Parameters.AddWithValue("@NOW_ZONE_IDX", 130); // mapa inicial
                cmd.Parameters.AddWithValue("@INIT_POS_LAYER", 255); // layer inicial
                cmd.Parameters.AddWithValue("@REVIVE_ZONE_IDX", 130); // mapa inicial - revive
                cmd.Parameters.AddWithValue("@BASELEVEL", 1); // lv inicial
                cmd.Parameters.AddWithValue("@SPEEDMOVE", 4); // velocidade char inicial
                cmd.Parameters.AddWithValue("@STR", MyHero.Energia); // energia
                cmd.Parameters.AddWithValue("@DEX", MyHero.Agilidade); // agilidade
                cmd.Parameters.AddWithValue("@AIM", MyHero.Exatidao); // exaltidao
                cmd.Parameters.AddWithValue("@LUCK", MyHero.Sorte); // sorte
                cmd.Parameters.AddWithValue("@RES_FIRE", 0); 
                cmd.Parameters.AddWithValue("@RES_WATER", 0);
                cmd.Parameters.AddWithValue("@RES_EARTH", 0);
                cmd.Parameters.AddWithValue("@RES_WIND", 0);
                cmd.Parameters.AddWithValue("@RES_DEVIL", 0);
                cmd.Parameters.AddWithValue("@HC", 1);
                cmd.Parameters.AddWithValue("@HD", 1);
                cmd.Parameters.AddWithValue("@AP", 3);
                cmd.Parameters.AddWithValue("@DP", 3);
                cmd.Parameters.AddWithValue("@HP", 16); // hp auto calcule by ZoneServer
                cmd.Parameters.AddWithValue("@MP", 14); // mp auto calcule by ZoneServer
                DateTime localDate = DateTime.Now;
                string time = localDate.ToString("yyyy-MM-dd HH:mm:ss");
                //Console.WriteLine("Time: " + time);
                cmd.Parameters.AddWithValue("@REGDATE", time); // horario atual do servidor
                cmd.Parameters.AddWithValue("@AVATAR_HEAD", MyHero.avatar_head); // avatar head
                cmd.Parameters.AddWithValue("@LAST_LOGOUT_TIME", 0); // 0
                cmd.Parameters.AddWithValue("@SKILL_POINT", 0); // skill points iniciais




                MySqlDataReader reader = cmd.ExecuteReader();
                if(reader.RecordsAffected > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                reader.Close();
            }

            return result;

        }


        public static List<XMOB> GetAllMobsEquipedFromID(int id_idx, int hero_order)
        {
            List<XMOB> Mobs = new List<XMOB>();
            if (conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT hench_order, monster_type FROM u_hench_0 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_1 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_2 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_3 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_4 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_5 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_6 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_7 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_8 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 UNION SELECT hench_order, monster_type FROM u_hench_9 WHERE id_idx = @ID AND hero_order = @HERO_ORDER AND position = 0 order by hench_order asc;";
                cmd.Parameters.AddWithValue("@ID", id_idx);
                cmd.Parameters.AddWithValue("@HERO_ORDER", hero_order);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        XMOB Temp = new XMOB();
                        Temp.hench_order = reader.GetInt32(0);
                        Temp.monster_type = reader.GetInt32(1);
                        Mobs.Add(Temp);
                    }
                } else { Mobs = new List<XMOB>(); }
                reader.Close();
            }

            return Mobs;
        }

        


        public static List<XHERO> GetAllHeroesFromID(int id_idx)
        {
            List<XHERO> Heroes = new List<XHERO>();
            //Console.WriteLine("Gettind characters!");
            if(conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select hero_order, serial, class, name, hero_type, baselevel, avatar_head, status, status_time from u_hero WHERE id_idx = @id ORDER BY hero_order asc;";
                cmd.Parameters.AddWithValue("@id", id_idx);


                MySqlDataReader reader = cmd.ExecuteReader();
                //Console.WriteLine("Columns:" + reader.FieldCount);
                while(reader.Read())
                {
                    XHERO temp = new XHERO();
                    temp.id_idx = id_idx;
                    temp.hero_order = reader.GetInt32(0);
                    temp.serial = reader.GetInt64(1);
                    temp.hero_class = reader.GetInt32(2);
                    temp.name = reader.GetString(3);
                    temp.hero_type = reader.GetInt32(4);
                    temp.baselevel = reader.GetInt32(5);
                    temp.avatar_head = reader.GetInt32(6);
                    temp.status = reader.GetInt32(7);

                    DateTime status_time = reader.GetDateTime(reader.GetOrdinal("status_time"));
                    temp.status_time = (int)ClientFunctions.GetTotalSecondsFromDateTime(status_time);
                    // Console.WriteLine("Status_time: " + ClientFunctions.GetTotalSecondsFromDateTime(status_time));
                    //Console.WriteLine("Status_time: " + status_time.ToString("yyyy - MM - dd HH: mm:ss"));

                    Heroes.Add(temp);
                }

                reader.Close();
            }



            return Heroes;
        }
        public static int GetCharactersCount(int id_idx)
        {
            int result = 0;
            if (conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select count(name) from u_hero where id_idx = @id;";
                cmd.Parameters.AddWithValue("@id", id_idx);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
                reader.Close();

            }
            return result;
        }





    }
}
