using System;
using System.Collections.Generic;
using Impinj.OctaneSdk;

namespace RfTools.Core
{
    public class RfidOptions
    {
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutputFileName { get; set; }

        /// <summary>
        /// 阅读器IP地址或域名
        /// </summary>
        public string HostName { get; set; } = "192.168.100.169";

        /// <summary>
        /// 发送功率
        /// </summary>
        public double TxPowerInDbm { get; set; } = 25;

        /// <summary>
        /// 接收灵敏度
        /// </summary>
        public double RxSensitivityInDbm { get; set; } = -70;

        /// <summary>
        /// 阅读器模式
        /// </summary>
        public ReaderMode ReaderMode { get; set; } = ReaderMode.AutoSetDenseReader;

        /// <summary>
        /// 目标标签掩码
        /// </summary>
        public string TargetMask { get; set; }

        /// <summary>
        /// 是否报告信号强度
        /// </summary>
        public bool ReportRssi { get; set; } = true;

        /// <summary>
        /// 是否报告相位
        /// </summary>
        public bool ReportPhase { get; set; } = true;

        /// <summary>
        /// 是否报告频率
        /// </summary>
        public bool ReportFrequency { get; set; } = true;

        /// <summary>
        /// 是否报告天线号码
        /// </summary>
        public bool ReportAntennaPortNumber { get; set; } = true;

        public Array ReaderModes=> System.Enum.GetValues(typeof(ReaderMode));
    }

}
