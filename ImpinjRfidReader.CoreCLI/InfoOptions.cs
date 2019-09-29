using CommandLine;

namespace ImpinjRfidReader.CoreCLI
{
    [Verb("info", HelpText = "获取阅读器信息")]
    public class InfoOptions
    {
        [Option('i', "ip", Default = "192.168.100.169", Required = false, HelpText = "Impinj阅读器的IP地址或域名")]
        public string HostName { get; set; }
    }
}
