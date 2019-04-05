using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// 点击物料列表时弹出此窗口，确认物料数量。
    /// 物料编号、物料名称
    /// 数量加减框
    /// 确认取消按钮
    /// 
    /// 接收物料名称、编号和数量，返回新的数量。
    /// </summary>
    public partial class ConfirmNumWindow : Window
    {
        private MaterialNumberCallback materialNumCallback;

        private MaterialNumber materialNumber;

        //第一个参数点击确认回调，第二个物料种类，第三个个数
        public ConfirmNumWindow(MaterialNumberCallback mNum, MaterialNumber materialNumber)
        {
            InitializeComponent();
            this.materialNumCallback = mNum;
            this.materialNumber = materialNumber;

            tbName.Text = materialNumber.m.mname;
            tbCode.Text = materialNumber.m.mid;
          
            tbNum.DataContext = this.materialNumber;
        }

        // 数量减一
        private void MinusNum(object sender, RoutedEventArgs e)
        {
            materialNumber.Count--;
        }

        // 数量加一
        private void AddNum(object sender, RoutedEventArgs e)
        {
            materialNumber.Count++;
        }

        // 确认，将值返回后关闭
        private void Confirm(object sender, RoutedEventArgs e)
        {
            materialNumCallback.Modify(materialNumber);
            Close();
        }

        // 点击取消，退出
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }

    // 用来绑定tbNum的数据model
    public class MaterialNumber:INotifyPropertyChanged
    {

        public material m { set; get; }

        private int count;

        public int Count
        {
            get { return this.count; }
            set
            {
                if (this.count != value)
                {
                    this.count = value;
                    this.NotifyPropertyChanged("Count");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class MaterialNumberCallback
    {
        // 委托。理解为方法指针，定义回调函数类型
        public delegate void NumHandler(MaterialNumber materialNumber);

        // 用来挂载被回调的函数
        public event NumHandler ModifyNumEvent;

        public void Modify(MaterialNumber materialNumber)
        {
            ModifyNumEvent(materialNumber);
        }
    }
}
