using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using Game_Manager_Server.MixMaster_API.Crypto;

namespace Game_Manager_Server.MixMaster_API.Network
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
                            bw.Write(XCRYPT.Decrypt(content, PubKeyIndex, XCRYPT.GameManagerServerPrivKey));
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
            foreach(byte b in data)
            {
                if(b != 0x00)
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

    public struct Header
    {
        public short len;
        public byte pubkey;
    }


    public class ReadPacket
    {
        byte[] buffer;
        Header Head;
        byte[] body;
        bool Initialized = false;

        public ReadPacket(byte[] data)
        {
            try
            {
                buffer = data;
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8))
                    {
                        short PacketLen = br.ReadInt16();
                        byte pubkey = br.ReadByte();
                        Head.len = PacketLen;
                        Head.pubkey = pubkey;

                        byte[] content = br.ReadBytes(PacketLen);

                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            using (BinaryWriter bw = new BinaryWriter(ms2))
                            {
                                byte PubKeyIndex = XCRYPT.GetPubKeyIndex(pubkey);
                                bw.Write(XCRYPT.Decrypt(content, PubKeyIndex, XCRYPT.LoginServerPrivkey));
                            }
                            ms2.Flush();
                            body = ms2.GetBuffer();
                            Array.Resize(ref body, content.Length);
                        }


                        /*
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            using (BinaryWriter bw = new BinaryWriter(ms2, Encoding.UTF8))
                            {
                                //byte PubKeyIndex = XCRYPT.GetPubKeyIndex(pubkey);
                                //bw.Write(XCRYPT.Decrypt(content, PubKeyIndex, XCRYPT.LoginServerPrivkey));
                                bw.Write(500);
                            }

                            byte[] buff = ms2.GetBuffer();
                            Array.Resize(ref buff, (int)ms2.Length);
                            body = buff;
                            
                        }
                        */
                    }
                }
                Initialized = true;
            }
            catch
            {
                Initialized = false;
                return;
            }
        }

        public Header GetHeader()
        {
            return Head;
        }

        public byte[] GetBody()
        {
            return body;
        }

        public byte GetPacketType()
        {
            return body[0];
        }

        public bool IsInitialized()
        {
            return Initialized;
        }








    }


    public class WritePacket
    {
        List<Byte> buffer = new List<byte>();
        Header head;
        int position = 0;

        public void AddByte(byte b)
        {
            buffer.Add(b);
            position += 1;
        }

        public void AddInt32(int value)
        {
            byte[] Intenger = BitConverter.GetBytes(value);
            for (int j = 0; j < Intenger.Length; j++)
            {
                buffer.Add(Intenger[j]);
                position += 1;
            }
        }

        public void AddInt16(short value)
        {
            byte[] Intenger = BitConverter.GetBytes(value);
            for (int j = 0; j < Intenger.Length; j++)
            {
                buffer.Add(Intenger[j]);
                position += 1;
            }
        }

        public void AddString(string value)
        {
            byte[] MsgInBytes = Encoding.UTF8.GetBytes(value);
            for (int j = 0; j < MsgInBytes.Length; j++)
            {
                buffer.Add(MsgInBytes[j]);
                position += 1;
            }
        }

        public byte[] GetPacket()
        {
            return buffer.ToArray();
        }
    }
}

