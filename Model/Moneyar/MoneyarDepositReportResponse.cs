using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarDepositReportResponse
    {
        public List<StatementBean> statementBeans { get; set; }
        public class StatementBean : DepositDetail
        {

        } 

        public enum DepositDetailState
        {
            [Description("اماده اعمال")]
            to_apply = 0,
            [Description("اعمال شده")]
            applied = 1,
            [Description(" خطا اعمال")]
            apple_error = -1,
            [Description(" بازگشت وجه")]
            refund = 2

        }

        public partial class DepositDetail : SRLCore.Model.CommonProperty
        {
            public string agentBranchCode { get; set; }
            public string agentBranchName { get; set; }
            public long balance { get; set; }
            public string branchCode { get; set; }
            public string branchName { get; set; }
            public DateTime date { get; set; }
            public string description { get; set; }
            public string referenceNumber { get; set; }
            [Required]
            public string serial { get; set; }
            public long transferAmount { get; set; }
            public int sequence { get; set; }

            public string pdate => SRL.Convertor.EnglishToPersianDate(date);
            [Required]
            public string bag_uid { get; set; }
            public DepositDetailState apply_state { get; set; }
            public string apply_error { get; set; }
            public DateTime? apply_date { get; set; }
            [NotMapped]
            public string serial_from_description
            {
                get
                {
                    string result = null;
                    if (description?.Length > 0)
                    {
                        var des = description.Replace("،", " ");
                        var des_parts = des.Split(" ");
                        var ser_q = des_parts.Where(x => x.StartsWith("1401") || x.StartsWith("1402") || x.StartsWith("1403")
                       || x.StartsWith("1404") || x.StartsWith("1405") || x.StartsWith("1406")
                        );
                        if (ser_q.Any())
                            result = ser_q.First().Replace(" ", "");
                    }
                    return result;
                }
            }
            [NotMapped]
            public string main_serial => string.IsNullOrWhiteSpace(serial) ? serial_from_description : serial;



        }
    }
}
