using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using ZoneServer.GameServerManager;

namespace ZoneServer.Network.GMS
{
    public class Receive
    {
        public void Process(byte[] data)
        {
            this.HandlerData(data);
        }

        private void HandlerData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short DataLen = br.ReadInt16();
                    byte packetType = br.ReadByte();
                    byte[] Data = br.ReadBytes(DataLen);
                    Console.WriteLine("[GMS] Receive packet");
                    ParseGMSInfo(Data, packetType);
                }
            }
        }

        private void ParseGMSInfo(byte[] packetData, byte type)
        {
            switch (type)
            {
                case 1:
                    ReadAuthGMS(packetData);
                    break;
                case 2:
                    ReceiveGMSToken(packetData);
                    break;
                default:
                    Console.WriteLine("[GMS] Packet type not found");
                    break;
            }
        }

        private void ReadAuthGMS(byte[] data)
        {

        }
        private void ReceiveGMSToken(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int id_idx = br.ReadInt32();
                    int _token = br.ReadInt32();
                    byte hero_order = br.ReadByte();

                    GameManager.TokenAccess token = new GameManager.TokenAccess(id_idx, _token, hero_order);

                    Init.game.InsertToken(token);  
                    Console.WriteLine("[GMS] Received token: " + token.id_idx + ":" + token.token + ":" + token.hero_order);
                }
            }
        }


    }
}
