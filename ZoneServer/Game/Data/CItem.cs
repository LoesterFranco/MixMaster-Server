using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class CItem
    {
        private XHERO hero;
        public uint id;
        public byte socket_type;
        public byte socket_num;
        public ushort count;
        public byte opt;
        public byte opt_level;
        public uint duration;
        public DateTime last_check_time;
        public byte synergy;
        public byte synergy_level;


        public CItem(XHERO hero)
        {

        }


    }
}
