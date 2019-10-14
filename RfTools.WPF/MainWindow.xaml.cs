using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RfTools.Core;
using Impinj.OctaneSdk;

namespace RfTools.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            RfidOption = new RfidOptions();
            Rfid = new RfidTool(RfidOption);
            RfidTags = new Dictionary<string,RfidTagInfo>();
        }

        #region 设置
        public RfidTool Rfid { get; set; }

        public RfidOptions RfidOption { get; set; } 

        public string IsConnectedStr => Rfid.IsConnected ? "已连接" : "未连接";

        public Dictionary<string, RfidTagInfo> RfidTags { get; set; }
        #endregion
        #region 阅读
        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            Rfid.TagsReported += Rfid_TagsReported;
            Rfid.BeginRead();
        }

        private void Rfid_TagsReported(ImpinjReader reader, TagReport report)
        {
            foreach (var tag in report.Tags)
            {
                string epc = tag.Epc.ToString();
                if (RfidTags.ContainsKey(epc))
                {
                    RfidTags[epc].ReadCount++;
                    if (tag.IsLastSeenTimePresent)
                    {
                        RfidTags[epc].LastSeenTime = tag.LastSeenTime.LocalDateTime;
                    }
                }
                else
                {
                    RfidTags.Add(epc, new RfidTagInfo(epc));
                }

            }
        }
        #endregion
    }
}
