using AccountHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace MaterialOT
{
    /// <summary>
    /// GetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GetWindow : Window
    {
        public GetWindow()
        {
            InitializeComponent();
        }

        public MmsContext context = new MmsContext();

        // 窗口加载完成，自动数据库查询当前
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource materialViewSource =
              ((System.Windows.Data.CollectionViewSource)(this.FindResource("materialViewSource")));
            context.category.Load();
            materialViewSource.Source = context.category.Local.ToBindingList();

        }
    }
}
