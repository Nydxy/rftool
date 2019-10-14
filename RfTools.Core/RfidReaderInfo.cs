using System;
using System.Collections.Generic;
using System.Text;

namespace RfTools.Core
{
    public class RfidReaderInfo
    {
        public string Address { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string ReaderModel { get; set; }
        public string FirmwareVersion { get; set; }
        public uint AntennaCount { get; set; }
        public bool IsConnected { get; set; }
    }
}
