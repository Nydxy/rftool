using CommandLine;
using Impinj.OctaneSdk;

namespace RfTools.CoreCLI
{
    [Verb("read", HelpText = "开始阅读标签")]
    public class ReadOptions
    {

        [Option('o', "filename", Required = false, HelpText = "输出文件名")]
        public string OutputFileName { get; set; }

        [Option('i', "ip", Default = "192.168.100.169", Required = false, HelpText = "Impinj阅读器的IP地址或域名")]
        public string HostName { get; set; }

        [Option('t', "TxPowerInDbm", Default = 25, Required = false, HelpText = "发送功率调整(默认为25dbm)")]
        public double TxPowerInDbm { get; set; }

        [Option('r', "RxSensitivityInDbm", Default = -70, Required = false, HelpText = "接收灵敏度(默认为-70dbm)")]
        public double RxSensitivityInDbm { get; set; }

        [Option('m', "ReaderMode", Default = ReaderMode.AutoSetDenseReader, Required = false, HelpText = "阅读模式")]
        public ReaderMode ReaderMode { get; set; }

        [Option('s', "TargetMask", Required = false, HelpText = "目标标签掩码(e.g. 2019 0000 0000 0000 0000 0001)")]
        public string TargetMask { get; set; }

        [Option("Rssi", Required = false, Default = true, HelpText = "是否记录RSSI")]
        public bool ReportRssi { get; set; } = true;

        [Option("Phase",Required = false, Default = true, HelpText = "是否记录phase")]
        public bool ReportPhase { get; set; } = true;

        [Option("Freq", Required = false, Default = true, HelpText = "是否记录Frequency")]
        public bool ReportFrequency { get; set; } = true;

        [Option("Ant", Required = false, Default = true, HelpText = "是否记录AntennaPortNumber")]
        public bool ReportAntennaPortNumber { get; set; } = true;
    }
}
