using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;


namespace ZoneServer.Network.ZS
{
    public struct XHERO
    {
        public int id_idx;
        public byte hero_order;
        public double serial;
        public byte Class;
        public string name;
        public byte hero_type;
        public short now_zone_idx;
        public byte now_zone_x;
        public byte now_zone_y;
        public byte moving_to_x;
        public byte moving_to_y;
        public short init_pos_layer;
        public short revive_zone_idx;
        public short baselevel;
        public int gold;
        public int attr;
        public long exp;
        public byte speed_move;
        public short speed_attack;
        public short speed_skill;
        public int str;
        public int dex;
        public int aim;
        public int luck;
        public short ap;
        public short dp;
        public short hc;
        public short hd;
        public int hp;
        public int mp;
        public int maxhp;
        public int maxmp;
        public int abil_freepoint;
        public short res_fire;
        public short res_water;
        public short res_earth;
        public short res_wind;
        public short res_devil;
        public byte ign_att_cnt;
        public DateTime regdate;
        public short avatar_head;
        public short avatar_body;
        public short avatar_foot;
        public int return_time;
        public byte status;
        public DateTime status_time;
        public short nickname;
        public DateTime last_logout_time;
        public int skill_point;
        public byte login;
    }

    public struct UserData
    {
        public XHERO Hero;
        public List<Structs.gamedata.QuestLog> QuestsLog;
        public List<Structs.gamedata.u_hench> MyHenchs;
    }

    public struct Account
    {
        public int id_idx;
        public int token;
        public bool IsValid;
    }

    public class Client
    {
        public int ID;
        public Socket socket;
        public const int BufferSize = 4096;
        public byte[] buffer = new byte[BufferSize];
        public Account account;
        public UserData data;


        public Client(int id, Socket Client)
        {
            this.ID = id;
            this.socket = Client;
        }
    }


    public class XCLIENT
    {
        public static List<Client> Clients = new List<Client>();

        public static bool IsClientIdExist(int clientID)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].ID == clientID)
                {
                    return true;
                }
            }
            return false;
        }

        public static Client GetClientFromID(int clientID)
        {
            for (int i = 0; 1 < Clients.Count; i++)
            {
                if (Clients[i].ID == clientID)
                {
                    return Clients[i];
                }
            }
            return null;
        }

        public static int GetClientIndexInList(int clientID)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].ID == clientID)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void RemoveClientFromList(Client MyClient)
        {
            Clients.Remove(MyClient);
        }

        public static void DisconnectClientFromID(int ClientID)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].ID == ClientID)
                {
                    Clients[i].socket.Close();
                    Clients[i].socket = null;
                    Clients.RemoveAt(i);
                }
            }
        }

        public static void DisconnectAll()
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].socket.Close();
                Clients[i].socket = null;
                Clients.RemoveAt(i);
            }
        }

    }


}
