using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;

namespace Game_Manager_Server.MixMaster_API.Database
{
    public class Member
    {
        static string myConnectionString = "Persist Security Info=False;server=" + MyInfo.MEMBER_HOST + ";uid=" + MyInfo.MEMBER_USER + ";pwd="+MyInfo.MEMBER_PASS+";database="+MyInfo.MEMBER_NAME+";";
        static MySqlConnection conn;


        public static bool ConnectMember()
        {
            conn = new MySqlConnection(myConnectionString);
            try
            {
                conn.Open();
                Console.WriteLine("[I] Connect to Member DB Successfully...");
                return true;
            }
            catch
            {
                return false;
            }
        }


        


        private static bool LoginExist(string username)
        {
            bool response = false;
            if(conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Player WHERE PlayerID='"+username+"';";

                MySqlDataReader datareader = cmd.ExecuteReader();
                if (datareader.HasRows)
                {
                    datareader.Close();
                    response = true;
                }
                else
                {
                    datareader.Close();
                    response = false;
                }
            }

            return response;
        }


       
        private static bool UserIsBanned(string username)
        {
            bool response = false;
            if(conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Player WHERE PlayerID='" + username + "' AND Block='GAME';";

                MySqlDataReader datareader = cmd.ExecuteReader();
                if(datareader.HasRows)
                {
                    response = true;
                    datareader.Close();
                    return response;
                }
                else
                {
                    response = false;
                    datareader.Close();
                    return response;
                }
            }
            return response;
        }

        public static int CheckLogin(Credentials MyCredential)
        {
            int response = 0;
            if(conn.State == ConnectionState.Open)
            {
                if(LoginExist(MyCredential.Username))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Player WHERE PlayerID='" + MyCredential.Username + "'" + " AND Passwd=password('" + MyCredential.Password + "');";
                    MySqlDataReader datareader = cmd.ExecuteReader();
                    bool loginExist = false;
                    bool loginBanned = false; 

                    if (datareader.HasRows)
                    {
                        // password correct
                        loginExist = true;
                        datareader.Close();
                    }
                    else
                    {
                        // password incorrect
                        response = 1;
                        datareader.Close();
                        return response;
                    }

                    loginBanned = UserIsBanned(MyCredential.Username);
                    if(loginExist && loginBanned == false) // okay - Login is valid
                    {
                        //Console.WriteLine("Login is valid! Next etap - GMS");
                        response = 3;
                    }
                    else if(loginExist && loginBanned) // no okay - login is banned
                    {
                        response = 4;
                    }
                }
                else
                {
                    // username and password don't exist
                    response = 2;
                    return response;
                }

                
            }


            return response;
        }



    }
}
