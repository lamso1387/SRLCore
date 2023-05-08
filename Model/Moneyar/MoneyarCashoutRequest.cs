using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarCashoutRequest
    {
        public long Amount { get; set; }
        public string Description { get; set; }
        public Srcuser SrcUser { get; set; }

        public class Srcuser
        {
            public string uid { get; set; }
            public string IbanNumber { get; set; }
        }

    }
}
