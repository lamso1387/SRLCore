using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarCompletedCashoutReportRequest : SRLCore.Model.WebRequest
    {
        public string sourcephonenumber { get; set; }
        public string destiban { get; set; }
        public string Iban { get; set; }


        public string Startdate { get; set; }
        public string Enddate { get; set; }

        public DateTime Startdate_en => SRL.Convertor.PersianDateStringToEnglishDate(Startdate, SRL.Convertor.DateFromTo.from);
        public DateTime Enddate_en => SRL.Convertor.PersianDateStringToEnglishDate(Enddate, SRL.Convertor.DateFromTo.to);


    }
}
