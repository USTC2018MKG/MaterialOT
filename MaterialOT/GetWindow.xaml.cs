using AccountHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

       

        // 窗口加载完成，自动数据库查询当前可用库存。每页当前20
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.material.Load();
            lvMaterials.ItemsSource = context.material.Local.ToBindingList();
        }

        // 通过搜索选择物料
        private void SearchClick(object sender, RoutedEventArgs e)
        {
              List<material> dataSource = context.material.Where(x => x.mid.Contains(tbForSearch.Text)).ToList();
            // dataSource = (from m in context.material where m.mid.Contains(tbForSearch.Text) select m)
            //                               .Take(20).ToBindingList();
            lvMaterials.ItemsSource = dataSource;
        }

        // 物料列表中的条目被点击
        private void MaterialItemClicked(object sender, SelectionChangedEventArgs e)
        {
            new ConfirmNumWindow().Show();
        }
    }
}
