using System;

namespace Domain
{
    [Serializable]
    public class Account
    {
        public int ID_NO { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public int P_OUT { set; get; }
    }
}