using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarCashoutReportResponse
    {
        public List<TRXs> transactions { get; set; }

        public class TRXs
        {
            public long amount { get; set; }
            public string sourceaccountuid { get; set; }
            public string sourcephonenumber { get; set; }
            public DateTime trxdatetime { get; set; }
            public string transactionuid { get; set; }
            public string referencenumber { get; set; }
            public string destiban { get; set; }
            public string description { get; set; }
            public DateTime? bankdatetime { get; set; }

            public string bankdatetime_persian => bankdatetime == null ? null : bankdatetime == DateTime.MinValue ? null : SRL.Convertor.EnglishToPersianDateTimeString((DateTime)bankdatetime);
            public string tr_referencenumber => $"tr:{referencenumber}";
        }


    }
}
