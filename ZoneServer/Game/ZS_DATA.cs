using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZoneServer.GameServerManager.Data;

namespace ZoneServer.GameServerManager
{
    public class ZS_DATA
    {
        private Socket conn;
        public bool AUTHENTICATED;
        public byte HERO_ORDER;
        public int ID_IDX;
        public bool LOGGED;

        public Socket GetConnectionAtributes()
        {
            return this.conn;
        }
        public void SetConnectionAtributes(Socket s)
        {
            this.conn = s;
        }

        public XHERO hero;

        public ZS_DATA(Socket s)
        {
            this.conn = s;
            this.AUTHENTICATED = false;
            this.HERO_ORDER = 0;
            this.ID_IDX = 0;
            this.LOGGED = false;
        }

        public void Create_ZSData()
        {
            hero = new XHERO(this);
            hero.LoadThisHero();
            hero.LoadMyHenchs();
            hero.LoadMyQuests();
            hero.LoadMySkills();
            hero.LoadMixSkill();
            hero.LoadItems();
        }

        public void SendMyInfo()
        {
            hero.SendThisHero();
            hero.SendThisHenchs();
            hero.SendThisQuestLog();
            hero.SendThisSkills();
            hero.SendThisMixSkill();
            hero.SendMySkillsPoints();
            hero.SendThisItems();
        }

        public void Update(long gameTime)
        {
            hero.ProcMove(gameTime);
        }


    }
}
