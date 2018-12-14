using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Login_Server
{
    public class MyInfo
    {
        public static string GMS_BIND_PASSWORD = "default_password";
        // gamemanager server properties
        public static byte gms_servers_count;
        public static byte gms_serverip_count;
        public static string server_name;
        public static string gms_serverip;
        public static short gms_port;
        public static short players_online;

        public static bool GMS_HANDSHAKE_SUCCESS = false;


        public static IPAddress MyIP;
        public static int MyPort;
        public static float MyVersion;
        public static byte MyPrivateKey;
        public static int MaxConnections;
        public static float MyVersionLowLimit;
        public static string FTPAdr;

        // GameManagerServer
        public static int GMS_SERVER;
        public static IPAddress GMS_SERVER_IP;
        public static int GMS_PORT;



        // Member connection configuration
        public static string mysql_host;
        public static int mysql_port;
        public static string mysql_user;
        public static string mysql_pass;
        public static string mysql_dbname;



        public static void PrintConfig()
        {
            ConsoleColor OldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("MYVERSION : " + MyVersion);
            Console.WriteLine("GMS_SERVER : " + GMS_SERVER);
            Console.WriteLine("LOGIN_SERVER_IP : " + MyIP.ToString());
            Console.WriteLine("LOGIN_SERVER_PORT : " + MyPort);
            Console.WriteLine("GAME_SERVER_IP : " + GMS_SERVER_IP.ToString());
            Console.WriteLine("GAME_SERVER_PORT : " + GMS_PORT);
            Console.WriteLine("LOGIN_DB_IP : " + mysql_host);
            Console.WriteLine("LOGIN_DB_PORT : " + mysql_port);
            Console.WriteLine("LOGIN_DB_NAME : " + mysql_dbname);
            Console.WriteLine("ACCESS CONTROL FLAG : LEVEL 4");
            Console.WriteLine("LOW_LIMIT : " + MyVersionLowLimit);
            Console.WriteLine("./ftplist.cfg # of FTP_SERVER : 1");
            Console.WriteLine("FTP SERVER URL : " + FTPAdr);
            Console.ForegroundColor = OldColor;

        }

        public static bool LoadMyInfo()
        {
            try
            {
                // LoginServer properties
                MyIP = IPAddress.Parse("127.0.0.1");
                MyPort = 22005;
                MyVersion = 3.86000f;
                MyVersionLowLimit = 3.86000f;
                MyPrivateKey = 102;
                MaxConnections = 1200;
                FTPAdr = "www.aerocybercurse.com";

                //GMS bind
                GMS_SERVER = 1;
                GMS_SERVER_IP = IPAddress.Parse("127.0.0.1");
                GMS_PORT = 32005;

                // mysql preferences
                mysql_host = "localhost";
                mysql_port = 3306;
                mysql_user = "root";
                mysql_pass = "toor";
                mysql_dbname = "Member";

                PrintConfig();
                return true;
            }
            catch
            {
                // report error in a log 
                return false;
            }
        }
    }
}
