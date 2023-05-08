using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Moneyar
{
    public class MoneyarResponse<T> : MoneyarResponse
    {
        public T data { get; set; }
    }
}
