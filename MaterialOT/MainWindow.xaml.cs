using AccountHelper;
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
using System.Windows.Threading;

namespace MaterialOT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private DispatcherTimer ShowTimer;

        public MainWindow()
        {
            InitializeComponent();

            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();
        }

        // 点击领料。
        private void GetMaterials(object sender, RoutedEventArgs e)
        {
            using (MmsContext context = new MmsContext())
            {
                if (!Account.Instance.IsLogin())
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                }
                
            }
        }

        // 定时更新时间
        private void ShowCurTimer(object sender, EventArgs e)
        {
            this.labelTime.Content = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
            this.labelTime.Content += " ";
            this.labelTime.Content += DateTime.Now.ToString("yyyy年MM月dd日");
            this.labelTime.Content += " ";
            this.labelTime.Content += DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
