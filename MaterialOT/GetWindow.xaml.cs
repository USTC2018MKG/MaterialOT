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

        private ObservableCollection<MaterialNumber> choosedMaterials;

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
            MaterialNumberCallback callback = new MaterialNumberCallback();
            callback.ModifyNumEvent += OnGetConfirmBack;
            MaterialNumber materialNumber = new MaterialNumber { m = lvMaterials.SelectedItem as material, Count = 1 };
            new ConfirmNumWindow(callback, materialNumber).Show();
        }

        // 确认添加物料数量后返回
        public void OnGetConfirmBack(MaterialNumber materialNumber)
        {
            if (choosedMaterials == null)
            {
                choosedMaterials = new ObservableCollection<MaterialNumber>();
            }

            // 如果已经添加了，需要修改数量而不是再次添加。
            foreach (MaterialNumber c in choosedMaterials)
            {
                if (c.m.mid.Equals(materialNumber.m.mid))
                {
                    c.Count = materialNumber.Count;
                    return;
                }
            }

            choosedMaterials.Add(materialNumber);
            lvChoosed.ItemsSource = choosedMaterials;
        }

        // TODO 修改添加物料数量后返回
        public void OnModifiedNumBack(material m, int num)
        {

        }

        // 确认选择的物料，生成取货订单
        private void ConfirmChoosed(object sender, RoutedEventArgs e)
        {
            if (!Account.Instance.IsLogin())
            {
                MessageBox.Show("缺少登录信息，请重试");
                return;
            }

            string id = System.Guid.NewGuid().ToString("N");

            // TODO 等待嵌入式设备返回操作成功信息

            foreach (MaterialNumber materialNumber in choosedMaterials)
            {
                // 若提交订单时，可用数量不够
                material materialLeft = context.material.Find(materialNumber.m.mid);
                if (materialLeft.rest < materialNumber.Count)
                {
                    MessageBox.Show("编号：" + materialLeft.mid + " 库存不足！");
                    return;
                }
                out_item outItemNew = new out_item() { out_id = id, mid = materialNumber.m.mid, num = materialNumber.Count };
                context.out_item.Add(outItemNew);

                // 修改物料表中的可用数量
                materialLeft.rest -= materialNumber.Count;
                context.Entry(materialLeft).State = EntityState.Modified;
            }

            out_order order = new out_order() { out_id = id, out_time = DateTime.Now, employee_id = Account.Instance.GetUser().emplyee_id };
            context.out_order.Add(order);

            context.SaveChanges();

            // 刷新界面，并且提示成功
            MessageBox.Show("取料成功!");
            lvMaterials.ItemsSource = context.material.ToList();
            // 清空已选择
            choosedMaterials.Clear();
        }
    }
}
