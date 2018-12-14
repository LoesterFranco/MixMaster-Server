using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Game_Manager_Server
{
    public class MyInfo
    {
        public static int ORDER;
        public static int GMS_NUM;
        public static string SERVER_NAME;
        public static float EXE_VERSION;
        public static byte GC_PRIVATE_KEY;
        public static int MAX_CONNECTIONS;

        // LGS
        public static string LGS_PASSWORD = "password_default";
        public static IPAddress LS_IP;
        public static short LS_PORT;

        // Client
        public static IPAddress MY_IP;
        public static short MY_PORT;

        // ZS bind
        public static IPAddress ZS_BIND_IP;
        public static short ZS_BIND_PORT;

        //gamedata db - game db
        public static string GAMEDB_USER;
        public static string GAMEDB_PASS;
        public static string GAMEDB_HOST;
        public static short  GAMEDB_PORT;
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

        public static bool LoadMyInfo()
        {
            try
            {
                ORDER = 0;
                GMS_NUM = 21;
                SERVER_NAME = "MixMaster remake";
                EXE_VERSION = 15.04f;
                GC_PRIVATE_KEY = 0x29;
                MAX_CONNECTIONS = 1200;

                // lgs
                LS_IP = IPAddress.Parse("127.0.0.1");
                LS_PORT = 32005;

                // Client
                MY_IP = IPAddress.Parse("127.0.0.1");
                MY_PORT = 21286;

                // ZS
                ZS_BIND_IP = IPAddress.Parse("127.0.0.1");
                ZS_BIND_PORT = 30186;

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
                Console.WriteLine("[*] Info Loaded successfully!");
                return true;
            }
            catch
            {
                return false;
            }
            
        }


    }
}
