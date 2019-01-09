using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ZoneServer.Network
{
    public struct GamePacket
    {
        public byte[] data;
        public Socket s;
        public GamePacket(byte[] data, Socket s)
        {
            this.data = data;
            this.s = s;
        }
    }


    public class XPacket
    {
        public short length;
        public byte[] data;
        public Client client;

        public XPacket(short Len, byte[] Data, Client Client)
        {
            length = Len;
            data = Data;
            client = Client;
        }
    }
}
