using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarTransactionDetailsReportRequest : SRLCore.Model.WebRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کیف پول")]
        public string uid { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("تاریخ شروع")]
        public string startdate { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("تاریخ پایان")]
        public string enddate { get; set; }


    }
}
