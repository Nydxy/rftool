using System;
using System.Collections.Generic;
using System.Text;

namespace RfTools.Core
{
    public class RfidTagInfo
    {
        public RfidTagInfo(string ePC="", uint readCount=0)
        {
            Epc = ePC;
            ReadCount = readCount;
        }

        public string Epc { get; set; }
        public uint ReadCount { get; set; }
        public DateTime LastSeenTime { get; set; }
    }
}
