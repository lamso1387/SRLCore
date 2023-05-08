using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class CashoutTransactionReportResponse
    {
        public List<Transactions> transactions { get; set; }
        public class Transactions
        {
            public long amount { get; set; }
            public string sourceaccountuid { get; set; }
            public string sourcephonenumber { get; set; }
            public DateTime trxdatetime { get; set; }
            public DateTime? bankdatetime { get; set; }
            public string status { get; set; }
            public string description { get; set; }
            public string referencenumber { get; set; }
            public string destiban { get; set; }
            public string transactionuid { get; set; }

        }
    }
}
