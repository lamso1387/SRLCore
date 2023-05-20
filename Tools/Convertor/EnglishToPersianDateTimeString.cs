using System;
using System.Collections.Generic;
using System.Globalization;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static string EnglishToPersianDateTimeString(DateTime d)
        {
            PersianCalendar pc = new PersianCalendar();
            string val = string.Format("{0}/{1}/{2} {3}:{4}:{5}",
                      pc.GetYear(d),
                      pc.GetMonth(d),
                      pc.GetDayOfMonth(d),
                      pc.GetHour(d),
                      pc.GetMinute(d),
                      pc.GetSecond(d));

            return val;
        }

    }
}
