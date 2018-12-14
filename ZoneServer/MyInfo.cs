using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ZoneServer
{
    public class MyInfo
    {

        public static bool LoadMyInfo()
        {
            try
            {
                ZS_PRIVATE_KEY = 107;
                ZC_MAXCLIENT = 5000;
                ZC_BIND_IP = IPAddress.Parse("127.0.0.1");
                ZC_BIND_PORT = 20165;
                GMS_NUM = 1;
                GMS_IP = IPAddress.Parse("127.0.0.1");
                GMS_PORT = 30186;

                MONEY_RATE = 1f;
                ITEM_RATE = 1f;
                CORE_RATE = 1f;
                EXP_RATE = 1f;


                // Gamedata DB
                GAMEDB_HOST = "localhost";
                GAMEDB_PORT = 3306;
                GAMEDB_USER = "root";
                GAMEDB_PASS = "toor";
                GAMEDB_NAME = "gamedata";

                //Member
                MEMBER_HOST = "localhost";
                MEMBER_PORT = 3306;
                MEMBER_USER = "root";
                MEMBER_PASS = "toor";
                MEMBER_NAME = "Member";

                // S_Data db
                SYSDB_HOST = "localhost";
                SYSDB_PORT = 3306;
                SYSDB_USER = "root";
                SYSDB_PASS = "toor";
                SYSDB_NAME = "S_Data";

                // LOG DB
                LOGDB_HOST = "localhost";
                LOGDB_PORT = 3306;
                LOGDB_USER = "root";
                LOGDB_PASS = "toor";
                LOGDB_NAME = "LogDB";

                // web account db
                Web_Accout_HOST = "localhost";
                Web_Accout_PORT = 3306;
                Web_Accout_USER = "root";
                Web_Accout_PASS = "toor";
                Web_Accout_NAME = "Web_Account";

                LogManager.CLogManager.WriteConsoleLog("[MYINFO] Loaded Successfully!", ConsoleColor.Green);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte ZS_PRIVATE_KEY;
        public static int ZC_MAXCLIENT;
        public static IPAddress ZC_BIND_IP;
        public static short ZC_BIND_PORT;
        public static short GMS_NUM;
        public static IPAddress GMS_IP;
        public static short GMS_PORT;

        public static float MONEY_RATE;
        public static float ITEM_RATE;
        public static float CORE_RATE;
        public static float EXP_RATE;

        public static bool LOGDB_ENABLE;
        public static bool LOGDB_TERM_MINUTE;

        //gamedata db - game db
        public static string GAMEDB_USER;
        public static string GAMEDB_PASS;
        public static string GAMEDB_HOST;
        public static short GAMEDB_PORT;
        public static string GAMEDB_NAME;

        //member db 
        public static string MEMBER_USER;
        public static string MEMBER_PASS;
        public static string MEMBER_HOST;
        public static short MEMBER_PORT;
        public static string MEMBER_NAME;

        //s_data db - System db
        public static string SYSDB_USER;
        public static string SYSDB_PASS;
        public static string SYSDB_HOST;
        public static short SYSDB_PORT;
        public static string SYSDB_NAME;

        // LOG DB
        public static string LOGDB_USER;
        public static string LOGDB_PASS;
        public static string LOGDB_HOST;
        public static short LOGDB_PORT;
        public static string LOGDB_NAME;

        // Web_Account db
        public static string Web_Accout_USER;
        public static string Web_Accout_PASS;
        public static string Web_Accout_HOST;
        public static short Web_Accout_PORT;
        public static string Web_Accout_NAME;


    }
}
