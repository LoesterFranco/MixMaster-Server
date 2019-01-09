using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.Database
{
    public class DatabaseManager
    {
        public DGameData gameData;
        public DSdata sysData;

        public DatabaseManager()
        {
            // Initialize GameData
            gameData = new DGameData("127.0.0.1", "root", "toor", "gamedata");
            gameData.Connect();
            gameData.LoadAll();

            // Initialize SData
            sysData = new DSdata("127.0.0.1", "root", "toor", "s_data");
            sysData.Connect();
            sysData.LoadAll();
        }

        // load GameData config
        // load SysData config

        // verify database connection
    }
}
