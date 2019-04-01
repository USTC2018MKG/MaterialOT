using MaterialOT;

namespace AccountHelper
{
    public class Account
    {
        private static volatile Account account;

        // 锁
        private static readonly object lockObject = new object();

        // 私有构造方法
        private Account() { }

        private static user mUser;

        public static Account Instance
        {
            get 
            {
                if (null == account)
                {
                    lock (lockObject)
                    {
                        if (null == account)
                        {
                            account = new Account();
                        }
                    }

                }
                return account;
            }
        }

        // 将登录成功的user信息保存下来
        public  void Login(user u)
        {
            mUser = u;
        }

        public  user GetUser()
        {
            return mUser;
        }

        public bool IsLogin()
        {
            return mUser != null && !string.IsNullOrEmpty(mUser.emplyee_id) 
                                 && !string.IsNullOrEmpty(mUser.user_pwd);
        }
    }
}