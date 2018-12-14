using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;

namespace ZoneServer.Database
{
    public class Member
    {
        static string myConnectionString = "Persist Security Info=False;server=" + MyInfo.MEMBER_HOST + ";uid=" + MyInfo.MEMBER_USER + ";pwd=" + MyInfo.MEMBER_PASS + ";database=" + MyInfo.MEMBER_NAME + ";";
        static MySqlConnection conn;

        public static bool ConnectGamedata()
        {
            conn = new MySqlConnection(myConnectionString);
            try
            {
                conn.Open();
                LogManager.CLogManager.WriteConsoleLog("[I] Connect to Member DB Successfully...", ConsoleColor.Green);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
