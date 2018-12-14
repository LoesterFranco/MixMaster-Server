using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Manager_Server.MixMaster_API.Network;
using Game_Manager_Server.MixMaster_API.Database;
using System.Threading;

namespace Game_Manager_Server
{
    public class init
    {
        public static List<ClientManager> Clients = new List<ClientManager>();
        public static List<SessionLGSToken> SessionLGS = new List<SessionLGSToken>();


        static void Main(string[] args)
        {
            Console.WriteLine("GAME MANAGER SERVER - REMAKE\n");
            Console.WriteLine("[*] Loading info ...");
            Console.Title = "MixMaster - GMS REMAKE";

            // load initial data
            if(!MyInfo.LoadMyInfo())
            {
                Console.WriteLine("Load MyInfo Failed....");
                return;
            }

            // start main socket listener
            if(!AsyncSocket.Start())
            {
                Console.WriteLine("CLIENT SOCKET CREATE FAILED!");
                return;
            }

            // start bind socket ZoneServer
            if(!ZS_Bind.Start())
            {
                Console.WriteLine("ZoneServer SOCKET CREATE FAILED");
                return;
            }

            // connect to LGS
            if(!LGS_Conn.Start())
            {
                Console.WriteLine("FAILED CONNECT TO LOGIN SERVER");
                return;
            }

            // connect db gamedata
            if (!gamedata.ConnectGamedata())
            {
                Console.WriteLine("FAILED TO LOAD GAMEDATA DB");
                Console.ReadKey();
                return;
            }

            // connect db member
            if(!Member.ConnectMember())
            {
                Console.WriteLine("FAILED TO LOAD MEMBER DB");
                return;
            }



            Thread.Sleep(2000);
            Console.CursorVisible = false;
            Console.WriteLine("\n---------------GameManagerServer---------------");
            


            Console.ReadLine();

           
        }
    }
}
