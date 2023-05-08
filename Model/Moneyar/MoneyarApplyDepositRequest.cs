using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarApplyDepositRequest : SRLCore.Model.WebRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = Constants.MessageText.RangeFieldErrorDynamic), DisplayName("مبلغ")]
        public long amount { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("شماره پیگیری")]
        public string tracenumber { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کیف مقصد")]
        public string accountuid { get; set; }
        public DateTime full_date { get; set; }
        public string date => full_date.ToShortDateString();


    }
}
