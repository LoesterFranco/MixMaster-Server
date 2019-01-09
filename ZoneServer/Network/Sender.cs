using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZoneServer.GameServerManager.Data;

namespace ZoneServer.Network
{
    public class Sender
    {
        private Queue<XPacket> packets = new Queue<XPacket>();
        private Queue<GamePacket> game_packets = new Queue<GamePacket>();

        public void AddPacket(XPacket packet)
        {
            packets.Enqueue(packet);
        }

        public void AddGamePacket(GamePacket gamePacket)
        {
            game_packets.Enqueue(gamePacket);
        }

        public void Update()
        {
            if (packets.Count > 0)
            {
                SendPacket(packets.Dequeue());
            }

            if(game_packets.Count > 0)
            {
                SendPacket(game_packets.Dequeue());
            }
        }

        private void SendPacket(XPacket packet)
        {
            packet.client.SendData(packet.data);
        }

        private void SendPacket(GamePacket packet)
        {
            Server.SendPacket(packet);
        }


        private void MakePacketAndSend(Client MyClient, byte[] content)
        {
            short ContentLenght = (short)content.Length;
            byte RandomPubKey = XCript.AddRandPubKey();

            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((short)ContentLenght);
                    bw.Write((byte)RandomPubKey);
                    bw.Write(XCript.Encrypt(content, XCript.GetPubKeyIndex(RandomPubKey), XCript.ZoneServerPrivKey));
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                XPacket packet = new XPacket((short)buffer.Length, buffer, MyClient);

                this.AddPacket(packet);
            }

        }

        public void MakePacketAndSend(Socket s, byte[] data)
        {
            short ContentLenght = (short)data.Length;
            byte RandomPubKey = XCript.AddRandPubKey();

            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((short)ContentLenght);
                    bw.Write((byte)RandomPubKey);
                    bw.Write(XCript.Encrypt(data, XCript.GetPubKeyIndex(RandomPubKey), XCript.ZoneServerPrivKey));
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                GamePacket packet = new GamePacket(buffer, s);

                this.AddGamePacket(packet);
            }
        }



        public void SendHello(Client MyClient)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write(0x65); // packet type
                    len = (int)bw.BaseStream.Length;
                }
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);
                this.MakePacketAndSend(MyClient, buffer);
            }
        }


        // FUNCTIONS PLAYERS 
       
    }

}
