using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarDepositReportRequest : SRLCore.Model.WebRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("تاریخ شروع")]
        public string pstartdate { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("تاریخ پایان")]
        public string penddate { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کیف پول")]
        public string accountuid { get; set; }

        public DateTime startdate => SRL.Convertor.PersianDateStringToEnglishDate(pstartdate, SRL.Convertor.DateFromTo.from);
        public DateTime enddate => SRL.Convertor.PersianDateStringToEnglishDate(penddate, SRL.Convertor.DateFromTo.to);

    }
}
