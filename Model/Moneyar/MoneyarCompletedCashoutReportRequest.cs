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

        public DateTime Startdate_en => Tools.ConvertorTools.PersianDateStringToEnglishDate(Startdate, Tools.ConvertorTools.DateFromTo.from);
        public DateTime Enddate_en => Tools.ConvertorTools.PersianDateStringToEnglishDate(Enddate, Tools.ConvertorTools.DateFromTo.to);


    }
}
