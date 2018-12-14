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
    public class SendData
    {
        public static void SendToGMS(Socket s, byte[] data)
        {
            try
            {
                s.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendTOGMSCallback), s);
            }
            catch
            {
                return;
            }
        }

        private static void SendTOGMSCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;
                s.EndReceive(ar);
            }
            catch
            {
                return;
            }
        }


        private static void MakePacketAndSend(Socket s, byte[] content)
        {
            short ContentLenght = (short)content.Length;
            //Console.WriteLine("Sending: " + ContentLenght + " | " + RandomPubKey);

            using (MemoryStream stream = new MemoryStream())
            {
                int len = 0;
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
                {
                    bw.Write((short)ContentLenght);
                    bw.Write(content);
                    len = (int)bw.BaseStream.Length;
                }
                stream.Flush();
                byte[] buffer = stream.GetBuffer();
                Array.Resize(ref buffer, len);

                SendToGMS(s, buffer);
            }

        }

    }
}
