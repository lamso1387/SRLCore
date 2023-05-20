using System;
using System.Collections.Generic;
using System.Globalization;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static string EnglishToPersianDate(DateTime? d_, string format = "{0}/{1}/{2}")
        {
            if (d_ == null) return null;
            DateTime d = (DateTime)d_;
            PersianCalendar pc = new PersianCalendar();
            string month = pc.GetMonth(d).ToString();
            string day = pc.GetDayOfMonth(d).ToString();
            string date = string.Format(format, pc.GetYear(d), month.Length == 1 ? "0" + month : month, day.Length == 1 ? "0" + day : day);
            return date;
        }
    }
}
