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

namespace RfTools.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }

        #region 设置
        public RfidTool Rfid { get; set; } = new RfidTool();
        public RfidOptions RfidOption { get; set; } = new RfidOptions();
        public string IsConnectedStr => Rfid.IsConnected ? "已连接" : "未连接";
        #endregion

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
