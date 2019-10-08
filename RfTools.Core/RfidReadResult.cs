using Impinj.OctaneSdk;
using System;
using System.Collections.Generic;

namespace RfTools.Core
{
    /// <summary>
    /// 阅读结果
    /// </summary>
    public class RfidReadResult
    {
        public int TotalCount { get; set; } = 0;

        public int ReportedCount { get; set; } = 0;

        public List<Tag> TagReports { get; set; }

        public Dictionary<string, int> DictinctTags { get; set; } = new Dictionary<string, int>();
    }
}
