using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarTransactionDetailsReportResponse<TransEnumType>
    {
        public List<TRXs> trx { get; set; }

        public class TRXs
        {
            public string id { get; set; }

            public string comment { get; set; }
            public string trxdetails { get; set; }
            public long deposit { get; set; }
            public long withdraw { get; set; }
            public string datestring { get; set; }
            public string sourceuid { get; set; }
            public string destinationuid { get; set; }
            public string comment_translate
            {
                get
                {
                    long order_code_num;
                    if (string.IsNullOrWhiteSpace(comment)) return "";
                    else if (long.TryParse(comment, out order_code_num)) return $"{Constants.MoneyarTrans.by_order} : {comment}";
                    else if (comment.Contains(":"))
                    {
                        List<string> spl = comment.Split(":").ToList();
                        string order_code = spl[1];
                        string transaction_type = spl[0];
                        string transaction_trans = "شماره پیگیری";

                        var list = Tools.ConvertorTools.EnumToDictionary(typeof(TransEnumType));

                        if (list.Where(x => x.Key == transaction_type).Any())

                            transaction_trans = list.First(x => x.Key == transaction_type).Value;

                        else if (transaction_type.Contains(Constants.MoneyarTrans.cashot_text))
                            transaction_trans = Constants.MoneyarTrans.bank_no;

                        return $"{transaction_trans} : {order_code}";
                    }
                    return comment;
                }
            }
        }


    }
}
