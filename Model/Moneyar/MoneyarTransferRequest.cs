using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarTransferRequest
    {
        public MoneyarTransferRequest() { }
        public long Amount { get; set; }
        public string Description { get; set; }
        public Srcuser SrcUser { get; set; }
        public Destuser DestUser { get; set; }

        public class Srcuser
        {
            public Srcuser() { }
            public string uid { get; set; }
        }
        public class Destuser
        {
            public Destuser() { }

            public string uid { get; set; }
        }
    }
}
