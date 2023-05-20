using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static DateTime PersianToEnglishDate(int year, int month, int day)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(year, month, day, pc);
            return dt;
        }

        public static DateTime PersianToEnglishDate(int year, int month, int day, int hour, int minute, int second, int milsec)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = pc.ToDateTime(year, month, day, hour, minute, second, milsec);

            return dt;
        }
    }
}
