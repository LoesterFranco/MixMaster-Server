using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace ZoneServer.Network.GMS
{
    public class ReceiveData
    {
        public static void ProcessGMSReceiveData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short DataLen = br.ReadInt16();
                    byte packetType = br.ReadByte();
                    byte[] Data = br.ReadBytes(DataLen);
                    Console.WriteLine("[GMS] Receive packet");
                    ParsingGMSDataReceived(packetType, Data);

                    br.Close();
                }
                ms.Close();
            }
        }

        private static void ParsingGMSDataReceived(byte packetType, byte[] packetData)
        {
            switch (packetType)
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

        private static void ReceiveGMSToken(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int id_idx = br.ReadInt32();
                    int token = br.ReadInt32();
                    byte hero_order = br.ReadByte();

                    ZS.GMSToken temp = new ZS.GMSToken();
                    temp.id_idx = id_idx;
                    temp.token = token;
                    temp.hero_order = hero_order;
                    ZS.ReceiveData.Tokens.Add(temp);

                    Console.WriteLine("[GMS] Received token: " + temp.id_idx + ":" + temp.token + ":" + temp.hero_order);
                }
            }

        }

        private static void ReadAuthGMS(byte[] data)
        {

        }
    }
}
