using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarResponse
    {
        public int? code { get; set; }
        public string message { get; set; }
        public bool IsOk => code == 0 ? true : false;
    }
}
