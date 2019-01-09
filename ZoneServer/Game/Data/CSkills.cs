using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class CSkill
    {
        private XHERO hero;
        public byte skillID;
        public byte skillLevel;
        public ushort skillPoint;
        public DateTime learningDate;


        public CSkill(XHERO hero)
        {
            this.hero = hero;
        }



    }
}
