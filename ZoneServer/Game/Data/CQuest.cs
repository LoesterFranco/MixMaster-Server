using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class CQuest
    {
        private XHERO hero;
        public ushort quest_index;
        public byte quest_state;


        public CQuest(XHERO hero)
        {
            this.hero = hero;
        }




    }
}
