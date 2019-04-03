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

            // 定时更新时间
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();
        }

        // 点击领料。检查登录状态。
        private void GetMaterials(object sender, RoutedEventArgs e)
        {
            using (MmsContext context = new MmsContext())
            {
                if (!Account.Instance.IsLogin())
                {
                    LoginCallback callback =  new LoginCallback();
                    callback.LoginEvent += LoginResult;
                    LoginWindow loginWindow = new LoginWindow(callback);
                    loginWindow.Show();
                }
                else
                {
                    GetWindow getWindow = new GetWindow();
                    getWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    getWindow.Show();
                }
                
            }
        }

        // 登录界面将登录结果返回，保存登录状态
        public void LoginResult(user u)
        {
            Account.Instance.Login(u);
            GetWindow getWindow = new GetWindow();
            getWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            getWindow.Show();
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

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public class LoginCallback
    {
        // 委托。理解为方法指针，定义回调函数类型
        public delegate void LoginHandler(user u);

        // 用来挂载被回调的函数
        public event LoginHandler LoginEvent;

        public void Login(user u)
        {
            LoginEvent(u);
        }
    }
}
