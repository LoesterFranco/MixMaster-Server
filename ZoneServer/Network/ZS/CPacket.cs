using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ZoneServer.Crypo;

namespace ZoneServer.Network.ZS
{
    public class PacketFunctions
    {
        public static byte[] GetPacketDataDecrypted(byte[] data)
        {
            byte[] Buffer;
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                {
                    short PacketLen = br.ReadInt16();
                    byte pubkey = br.ReadByte();

                    byte[] content = br.ReadBytes(PacketLen);

                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        using (BinaryWriter bw = new BinaryWriter(ms2))
                        {
                            byte PubKeyIndex = XCRYPT.GetPubKeyIndex(pubkey);
                            bw.Write(XCRYPT.Decrypt(content, PubKeyIndex, XCRYPT.ZoneServerPrivKey));
                        }
                        ms2.Flush();
                        Buffer = ms2.GetBuffer();
                        Array.Resize(ref Buffer, content.Length);
                    }
                }
            }
            return Buffer;
        }
        public static string ExtractStringFromBytes(byte[] data)
        {
            string result = "";
            foreach (byte b in data)
            {
                if (b != 0x00)
                {
                    result += (char)b;
                }
                else
                {
                    break;
                }
            }
            return result;
        }
    }

    public class CPacket
    {

    }
}
