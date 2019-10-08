using System;
using System.Collections.Generic;
using CommandLine;
using RfTools.Core;
using System.Threading;

namespace RfTools.CoreCLI
{
    partial class Program
    {

        static void Main(string[] args)
        {
            var exitcode = Parser.Default.ParseArguments<ReadOptions, InfoOptions>(args)
                .MapResult(
                (ReadOptions o) => Read(o),
                (InfoOptions o) => GetReaderInfo(o),
                error => 1
                );
        }

        static int Read(ReadOptions options)
        {
            var readtool = new RfidTool(o =>
            {
                o.HostName = options.HostName;
                o.OutputFileName = options.OutputFileName;
                o.ReaderMode = options.ReaderMode;
                o.ReportAntennaPortNumber = options.ReportAntennaPortNumber;
                o.ReportFrequency = options.ReportFrequency;
                o.ReportPhase = options.ReportPhase;
                o.ReportRssi = options.ReportRssi;
                o.RxSensitivityInDbm = options.RxSensitivityInDbm;
                o.TargetMask = options.TargetMask;
                o.TxPowerInDbm = options.TxPowerInDbm;
            });

            if (readtool.BeginRead())
            {
                Console.ReadLine();
                readtool.EndRead();
            }
            return 0;
        }

        static int GetReaderInfo(InfoOptions options)
        {
            return RfidTool.GetReaderInfo(options.HostName);
        }

        static void fuck()
        {
            RfidTool.Read(wait_for_stop: () => Thread.Sleep(1000),reader_address:"192.168.100.169");
        }
    }
}
