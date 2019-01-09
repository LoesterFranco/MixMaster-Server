using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoneServer.GameServerManager.Data;

namespace ZoneServer.GameServerManager
{
    public class GameManager
    {
        public struct TokenAccess
        {
            public int id_idx;
            public int token;
            public byte hero_order;
            public TokenAccess(int id_idx, int token, byte hero_order)
            {
                this.id_idx = id_idx;
                this.token = token;
                this.hero_order = hero_order;
            }
        }


        private List<TokenAccess> tokens;
        public SData sdata;
        public GameData gamedata;
        public MapManager mapManager;

        public GameManager()
        {
            sdata = new SData();
            gamedata = new GameData();
            mapManager = new MapManager();
            tokens = new List<TokenAccess>();
        }

        public void InsertToken(TokenAccess _token)
        {
            tokens.Add(_token);
        }

        public bool IsValidToken(TokenAccess _token)
        {
            for(int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].token == _token.token && tokens[i].id_idx == _token.id_idx && tokens[i].hero_order == _token.hero_order)
                    return true;
            }
            return false;
        }

        public static int GetDistance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }







    }
}
