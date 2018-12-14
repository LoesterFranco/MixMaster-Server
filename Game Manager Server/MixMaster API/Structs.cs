using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Manager_Server.MixMaster_API
{

    public struct Credentials
    {
        public string Username;
        public string Password;
    }


    public struct XMOB
    {
        public int monster_type;
        public int hench_order;
    }


    public struct XHERO
    {
        public int id_idx;
        public int hero_order;
        public long serial;
        public int hero_class;
        public string name;
        public int hero_type;
        public int baselevel;
        public int avatar_head;
        public short Energia ;
        public short Agilidade;
        public short Exatidao;
        public short Sorte;
        public int status_time;
        public int status;
    }
    public class Structs
    {
        
    }
}
