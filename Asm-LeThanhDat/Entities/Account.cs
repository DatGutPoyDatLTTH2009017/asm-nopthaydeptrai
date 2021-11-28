using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asm_LeThanhDat.Entities
{
    public class Account
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public int gender { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public int role { get; set; } // 1. user | 99. admin
        public string introduction { get; set; }

        public override string ToString()
        {
            return $"Name is {firstName} {lastName}, email {email}";
        }
    }
}
}
