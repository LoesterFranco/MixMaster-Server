using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ZoneServer.GameServerManager.Data;
using ZoneServer.GameServerManager;

namespace ZoneServer.Network
{
    public class Client
    {
        public EventHandler OnDisconnect;
        public int id;
        public Socket s;
        public byte[] buffer;
        public const int buffer_size = 2048;
        public int receivedBytes = 0;

        public ZS_DATA zs_data;


        public Client(int clientID, Socket socket)
        {
            this.id = clientID;
            this.s = socket;
            this.buffer = new byte[Client.buffer_size];
            this.zs_data = new ZS_DATA(this.s);
        }
        public void Disconnect()
        {
            if (s.Connected)
                s.Disconnect(false);
            s.Dispose();
            id = 0;
            this.OnDisconnect(this, EventArgs.Empty);
        }
        public void SendData(byte[] data)
        {
            try
            {
                this.s.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), this.s);
            }
            catch
            {
                return;
            }
        }
        private void SendCallback(IAsyncResult e)
        {
            Socket sock = (Socket)e.AsyncState;
            try
            {
                sock.EndSend(e);
            }
            catch
            {
                return;
            }
        }




    }
}
