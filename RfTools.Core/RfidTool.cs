using System;
using System.Collections.Generic;
using System.Text;
using Impinj.OctaneSdk;
using System.IO;

namespace RfTools.Core
{
    public class RfidTool
    {
        // Create an instance of the ImpinjReader class.
        private ImpinjReader reader;
        public RfidOptions option;

        /// <summary>
        /// 要保存的文件
        /// </summary>
        private StreamWriter file;
        public bool IsReading { get; private set; }

        /// <summary>
        /// 重定向标准输入输出流
        /// </summary>
        public event Log RedirectStandardOut;
        public delegate void Log(string str);

        /// <summary>
        /// 每次收到标签响应时调用
        /// </summary>
        public event ImpinjReader.TagsReportedHandler TagsReported;

        public RfidTool(RfidOptions readOptions)
        {
            reader = new ImpinjReader();
            option = readOptions;
            RedirectStandardOut += Console.WriteLine;
        }

        public RfidTool(Action<RfidOptions> func)
        {
            reader = new ImpinjReader();
            option = new RfidOptions();
            func(option);
            RedirectStandardOut += Console.WriteLine;
        }

        /// <summary>
        /// 开始阅读
        /// </summary>
        /// <returns>是否开始阅读</returns>
        public bool BeginRead()
        {
            try
            {
                if (!reader.IsConnected)
                {
                    RedirectStandardOut.Invoke("Connecting to reader...");
                    reader.Connect(option.HostName);
                    RedirectStandardOut.Invoke("Connection established.");
                }

                //获取该阅读器的默认设置，然后在其基础上修改
                Settings settings = reader.QueryDefaultSettings();

                // Send a tag report for every tag read.
                settings.Report.Mode = ReportMode.Individual;

                if (!string.IsNullOrEmpty(option.TargetMask))
                {
                    settings.Filters.TagFilter1.MemoryBank = MemoryBank.Epc;
                    settings.Filters.TagFilter1.BitPointer = BitPointers.Epc;
                    settings.Filters.TagFilter1.TagMask = option.TargetMask;
                    settings.Filters.TagFilter1.BitCount = option.TargetMask.Length * 4;
                    settings.Filters.Mode = TagFilterMode.OnlyFilter1;
                }

                //包含天线号码
                settings.Report.IncludeAntennaPortNumber = true;
                //报告信号强度
                settings.Report.IncludePeakRssi = option.ReportRssi;
                //报告相位
                settings.Report.IncludePhaseAngle = option.ReportPhase;
                //报告频率
                settings.Report.IncludeChannel = option.ReportFrequency;

                //阅读阅读模式
                settings.ReaderMode = option.ReaderMode;

                // 仅使用1号天线
                settings.Antennas.DisableAll();
                settings.Antennas.GetAntenna(1).IsEnabled = true;

                //设置传输功率和接收灵敏度
                settings.Antennas.GetAntenna(1).TxPowerInDbm = option.TxPowerInDbm;
                settings.Antennas.GetAntenna(1).RxSensitivityInDbm = option.RxSensitivityInDbm;

                // 应用设置
                reader.ApplySettings(settings);

                // 指定事件处理函数
                reader.TagsReported += (reader, report) =>
                {
                    foreach (Tag tag in report)
                    {
                        RedirectStandardOut.Invoke($"Antenna: {tag.AntennaPortNumber}, EPC: {tag.Epc}, RSSI: {tag.PeakRssiInDbm}, Phase: {tag.PhaseAngleInRadians}, Freq: {tag.ChannelInMhz}");
                        if (file != null)
                        {
                            file.WriteLine(string.Join(",",
                                tag.AntennaPortNumber.ToString(),
                                tag.Epc.ToString(),
                                tag.PeakRssiInDbm.ToString(),
                                tag.PhaseAngleInRadians.ToString(),
                                tag.ChannelInMhz.ToString()));
                        }
                    }
                };

                reader.TagsReported += TagsReported;

                //如果需要输出到文件，初始化文件流
                if (!string.IsNullOrEmpty(option.OutputFileName))
                {
                    file = new StreamWriter(option.OutputFileName);
                }

                // 开始阅读  （此方法为异步执行）
                RedirectStandardOut.Invoke("Start reading!");
                RedirectStandardOut.Invoke("----------------------------");
                reader.Start();
                IsReading = true;
                return true;
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                RedirectStandardOut.Invoke("Octane SDK exception: " + e.Message);
                return false;
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                RedirectStandardOut.Invoke("Exception : " + e.Message);
                return false;
            }

        }


        /// <summary>
        /// 停止阅读
        /// </summary>
        public void EndRead()
        {
            if (!IsReading) return;
            try
            {
                reader.Stop();
                RedirectStandardOut.Invoke("Stop reading!");

                if (!string.IsNullOrEmpty(option.OutputFileName))
                {
                    file.Close();
                    file.Dispose();
                }

                // 断开与阅读器的连接
                reader.Disconnect();
                RedirectStandardOut.Invoke("Connection end.");
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                RedirectStandardOut.Invoke("Octane SDK exception: " + e.Message);
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                RedirectStandardOut.Invoke("Exception : " + e.Message);
            }
        }

        public bool GetReaderInfo()
        {
            try
            {
                if (!reader.IsConnected)
                {
                    RedirectStandardOut.Invoke("Connecting to reader...");
                    reader.Connect(option.HostName);
                    RedirectStandardOut.Invoke("Connection established.");
                    RedirectStandardOut.Invoke("");
                }

                // Query the reader features and print the results.
                RedirectStandardOut.Invoke("Reader Features");
                RedirectStandardOut.Invoke("---------------");
                FeatureSet features = reader.QueryFeatureSet();
                RedirectStandardOut.Invoke($"Model name : {features.ModelName}");
                RedirectStandardOut.Invoke($"Model number : {features.ModelNumber}");
                RedirectStandardOut.Invoke($"Reader model : {features.ReaderModel.ToString()}");
                RedirectStandardOut.Invoke($"Firmware version : {features.FirmwareVersion}");
                RedirectStandardOut.Invoke($"Antenna count : {features.AntennaCount}\n");

                // Query the current reader status.
                RedirectStandardOut.Invoke("Reader Status");
                RedirectStandardOut.Invoke("---------------");
                Status status = reader.QueryStatus();
                RedirectStandardOut.Invoke($"Is connected : {status.IsConnected}");
                RedirectStandardOut.Invoke($"Is singulating : {status.IsSingulating}");
                RedirectStandardOut.Invoke($"Temperature : {status.TemperatureInCelsius}° C\n");

                // Query the current reader settings and print the results.
                RedirectStandardOut.Invoke("Reader Settings");
                RedirectStandardOut.Invoke("---------------");

                Settings settings = reader.QueryDefaultSettings();
                RedirectStandardOut.Invoke($"Reader mode : {settings.ReaderMode}");
                RedirectStandardOut.Invoke($"Search mode : {settings.SearchMode}");
                RedirectStandardOut.Invoke($"Session : {settings.Session}");


                for (ushort i = 1; i <= features.AntennaCount; i++)
                {
                    RedirectStandardOut.Invoke($"[Antenna {i.ToString()}]");
                    if (settings.Antennas.GetAntenna(i).MaxRxSensitivity)
                    {
                        RedirectStandardOut.Invoke("Rx sensitivity : Max");
                    }
                    else
                    {
                        RedirectStandardOut.Invoke($"Rx sensitivity : {settings.Antennas.GetAntenna(i).RxSensitivityInDbm} dBm");
                    }

                    if (settings.Antennas.GetAntenna(i).MaxTxPower)
                    {
                        RedirectStandardOut.Invoke("Tx power : Max");
                    }
                    else
                    {
                        RedirectStandardOut.Invoke($"Tx power : {settings.Antennas.GetAntenna(i).TxPowerInDbm} dBm");
                    }

                    RedirectStandardOut.Invoke("");
                }

                return true;
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                RedirectStandardOut.Invoke($"Octane SDK exception: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                RedirectStandardOut.Invoke($"Exception : {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取阅读器信息
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int GetReaderInfo(string address)
        {
            try
            {
                var reader = new ImpinjReader();
                reader.Connect(address);

                // Query the reader features and print the results.
                Console.WriteLine("Reader Features");
                Console.WriteLine("---------------");
                FeatureSet features = reader.QueryFeatureSet();
                Console.WriteLine("Model name : {0}", features.ModelName);
                Console.WriteLine("Model number : {0}", features.ModelNumber);
                Console.WriteLine("Reader model : {0}", features.ReaderModel.ToString());
                Console.WriteLine("Firmware version : {0}", features.FirmwareVersion);
                Console.WriteLine("Antenna count : {0}\n", features.AntennaCount);

                // Query the current reader status.
                Console.WriteLine("Reader Status");
                Console.WriteLine("---------------");
                Status status = reader.QueryStatus();
                Console.WriteLine("Is connected : {0}", status.IsConnected);
                Console.WriteLine("Is singulating : {0}", status.IsSingulating);
                Console.WriteLine("Temperature : {0}° C\n", status.TemperatureInCelsius);

                // Query the current reader settings and print the results.
                Console.WriteLine("Reader Settings");
                Console.WriteLine("---------------");

                Settings settings = reader.QueryDefaultSettings();
                Console.WriteLine("Reader mode : {0}", settings.ReaderMode);
                Console.WriteLine("Search mode : {0}", settings.SearchMode);
                Console.WriteLine("Session : {0}", settings.Session);

                if (settings.Antennas.GetAntenna(1).MaxRxSensitivity)
                {
                    Console.WriteLine("Rx sensitivity (Antenna 1) : Max");
                }
                else
                {
                    Console.WriteLine("Rx sensitivity (Antenna 1) : {0} dBm",
                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm);
                }

                if (settings.Antennas.GetAntenna(1).MaxTxPower)
                {
                    Console.WriteLine("Tx power (Antenna 1) : Max");
                }
                else
                {
                    Console.WriteLine("Tx power (Antenna 1) : {0} dBm",
                        settings.Antennas.GetAntenna(1).TxPowerInDbm);
                }

                Console.WriteLine("");
                return 0;
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
                return 1;
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", e.Message);
                return 1;
            }
        }

        public class RfidReadResult
        {
            public int TotalCount { get; set; }
            public int ReportedCount { get; set; }

        }
    }
}
