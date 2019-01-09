using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.GameServerManager
{
    public class XBATTLE_HENCH
    {
        public XHENCH_INFO hench_base;

        public XBATTLE_HENCH(XHENCH_INFO hench)
        {
            this.hench_base = hench;
        }

        public void SendRefreshStatus(XHERO hero, byte order)
        {
            if (order < 0 || order > 19) return;

            using (MemoryStream ms = new MemoryStream())
            {
                int packetlen = 0;
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)188); // packet type
                    bw.Write((byte)this.hench_base.hench_order);
                    bw.Write((ushort)00); // unknow
                    bw.Write((ushort)this.hench_base.str);
                    bw.Write((ushort)this.hench_base.dex);
                    bw.Write((ushort)this.hench_base.aim);
                    bw.Write((ushort)this.hench_base.luck);

                    bw.Write((ushort)this.hench_base.ap); // talvez
                    bw.Write((ushort)this.hench_base.dp); // talvez
                    bw.Write((ushort)this.hench_base.hc); // talvez
                    bw.Write((ushort)this.hench_base.hd); // talvez
                    bw.Write((ushort)50); // cured

                    packetlen = (int)bw.BaseStream.Length;
                }
                byte[] buffer = ms.GetBuffer();
                Array.Resize(ref buffer, packetlen);
                Init.server.sendManager.MakePacketAndSend(hero.zs_data.GetConnectionAtributes(), buffer);
            }
        }



        public object Clone()
        {
            return this.MemberwiseClone();
        }



    }
}
