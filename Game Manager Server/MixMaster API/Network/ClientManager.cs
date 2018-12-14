using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Game_Manager_Server.MixMaster_API.Network
{
    public struct SessionLGSToken
    {
        public string username;
        public int id_idx;
        public int LGS_Token;
    }

   




    public struct UserData
    {
        public string username;
        public float exe_ver;
        public int id_idx;
        public bool ExeVersionOK;
        public bool Authenticated;

        
        
    }

    public class ClientManager
    {
        public int id;
        public Socket _socket;
        public const int BufferSize = 4096;
        public byte[] buffer = new byte[BufferSize];
        public UserData data;

        public ClientManager(int id, Socket Client)
        {
            this.id = id;
            this._socket = Client;
        }
    }

    public class ClientFunctions
    {
        public static int GetTotalSecondsFromDateTime(DateTime time)
        {
            TimeSpan span = time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (int)span.TotalSeconds;
        }



        public static bool SearchUserIDInSession(int id_idx)
        {
            for(int i=0; i< init.SessionLGS.Count; i++)
            {
                if(init.SessionLGS[i].id_idx == id_idx)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TokenIsValidInSession(int id_idx, int Token, string username)
        {
            for (int i = 0; i < init.SessionLGS.Count; i++)
            {
                if(init.SessionLGS[i].id_idx == id_idx && init.SessionLGS[i].LGS_Token == Token && init.SessionLGS[i].username == username)
                {
                    return true;
                }
            }
            return false;
        }










        public static bool ClientIdExist(int clientID)
        {
            for(int i=0; i < init.Clients.Count; i++)
            {
                if(init.Clients[i].id == clientID)
                {
                    return true;
                }
            }
            return false;
        }

        public static ClientManager GetInstaceFromClientID(int clientID)
        {
            for(int i=0; 1 < init.Clients.Count; i++)
            {
                if(init.Clients[i].id == clientID)
                {
                    return init.Clients[i];
                }
            }
            return null;
        }


        public static int GetClientIndex(int clientID)
        {
            for(int i=0; i< init.Clients.Count; i++)
            {
                if(init.Clients[i].id == clientID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void RemoveClientFromInstance(ClientManager MyClient)
        {
            init.Clients.Remove(MyClient);
        }

        public static void DisconnectClientFromID(int ClientID)
        {
            for(int i=0; i < init.Clients.Count; i++)
            {
                if(init.Clients[i].id == ClientID)
                {
                    init.Clients[i]._socket.Close();
                    init.Clients[i]._socket = null;
                    init.Clients.RemoveAt(i);
                }
            }
        }

        public static void DisconnectAll()
        {
            for (int i = 0; i < init.Clients.Count; i++)
            {
                init.Clients[i]._socket.Close();
                init.Clients[i]._socket = null;
                init.Clients.RemoveAt(i);
            }
        }
    }

    
}
