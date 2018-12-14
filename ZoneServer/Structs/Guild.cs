using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoneServer.Structs
{
    public class Guild
    {
        public struct guild
        {
            public short GuildIdx;
            public string Name;
            public string Info;
            public string Cert;
            public DateTime EstablishDate;
            public short LimitCount;
            public byte Status;
            public DateTime MarkRegDate;
            public byte MarkRegCnt;
            public DateTime Dissolution;
            public int gold;
            public short HiringIdx;
            public DateTime CertDate;
            public DateTime InfoDate;
        }

        public struct guild_member
        {
            public int HeroIdx;
            public byte HeroOrder;
            public short GuildIdx;
            public short MemberID;
            public short Grade;
            public int Authority;
            //public DateTime UpgradeDate;
            public string Memo;
        }

    }

    
}
