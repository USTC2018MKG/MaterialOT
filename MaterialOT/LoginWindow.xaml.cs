﻿using AccountHelper;
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
using System.Windows.Shapes;

namespace MaterialOT
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class LoginWindow : Window
    {
        public LoginCallback loginCallback;

        public LoginWindow(LoginCallback callback)
        {
            InitializeComponent();
            this.loginCallback = callback;
        }

        private void login(object sender, RoutedEventArgs e)
        {
            using(MmsContext context = new MmsContext())
            {
                if (String.IsNullOrEmpty(tbUserName.Text.Trim()))
                {
                    MessageBox.Show("请输入员工号！");
                }

                if (String.IsNullOrEmpty(pbPwd.Password))
                {
                    MessageBox.Show("请输入登录密码！");
                }

                user userLogin = context.user.Where(u => u.emplyee_id == tbUserName.Text.Trim().ToString()
                                                        && u.user_pwd == pbPwd.Password.Trim().ToString())
                                                        .FirstOrDefault();
                if(userLogin != null)
                {
                    Account.Instance.Login(userLogin);
                    loginCallback.Login(userLogin);
                    Close();
                }
                else
                {
                    // 登录失败，清空密码栏
                    pbPwd.Password = "";
                    MessageBox.Show("员工号码或密码错，请重新登录！");
                }
            }
        }
    }
}
