using System;
using System.Collections.Generic;
using System.Globalization;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static string EnglishToPersianDateTime(DateTime date)
        {

            PersianCalendar p = new System.Globalization.PersianCalendar();
            int year = p.GetYear(date);
            int month = p.GetMonth(date);
            int day = p.GetDayOfMonth(date);
            int h = p.GetHour(date);
            int m = p.GetMinute(date);
            int s = p.GetSecond(date);
            string str = string.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, h, m, s);
            return str;
        }
    }
}
