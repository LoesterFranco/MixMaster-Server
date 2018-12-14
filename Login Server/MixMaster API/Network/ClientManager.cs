using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Login_Server.MixMaster_API.Network
{
    public struct UserData
    {
        public string username;
        public string password;
        public float Version;
        public int id_idx;
        public int GMS_TOKEN;
        public string blocked;
        public string last_ip_address;
        public bool VersionOK;
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
        public static bool ClientIdExist(int clientID)
        {
            for(int i=0; i < Init.Clients.Count; i++)
            {
                if(Init.Clients[i].id == clientID)
                {
                    return true;
                }
            }
            return false;
        }

        public static ClientManager GetInstaceFromClientID(int clientID)
        {
            for(int i=0; 1 < Init.Clients.Count; i++)
            {
                if(Init.Clients[i].id == clientID)
                {
                    return Init.Clients[i];
                }
            }
            return null;
        }


        public static int GetClientIndex(int clientID)
        {
            for(int i=0; i< Init.Clients.Count; i++)
            {
                if(Init.Clients[i].id == clientID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void RemoveClientFromInstance(ClientManager MyClient)
        {
            Init.Clients.Remove(MyClient);
        }

        public static void DisconnectClientFromID(int ClientID)
        {
            for(int i=0; i < Init.Clients.Count; i++)
            {
                if(Init.Clients[i].id == ClientID)
                {
                    Init.Clients[i]._socket.Close();
                    Init.Clients[i]._socket = null;
                    Init.Clients.RemoveAt(i);
                }
            }
        }

        public static void DisconnectAll()
        {
            for (int i = 0; i < Init.Clients.Count; i++)
            {
                Init.Clients[i]._socket.Close();
                Init.Clients[i]._socket = null;
                Init.Clients.RemoveAt(i);
            }
        }
    }

    
}
