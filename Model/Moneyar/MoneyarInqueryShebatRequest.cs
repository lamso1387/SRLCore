using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static SRLCore.Model.ValidationAttr;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarInqueryShebatRequest : SRLCore.Model.WebRequest
    {
        [Shaba(ErrorMessage = Constants.MessageText.FieldFormatErrorDynamic), DisplayName("شبا راننده اول")]
        public virtual string shaba { get; set; }


    }
}
