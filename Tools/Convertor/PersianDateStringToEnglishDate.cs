using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public enum DateFromTo
        {
            from,
            to
        }

        public static DateTime PersianDateStringToEnglishDate(string pdate, DateFromTo from_to)
        {
            int year = int.Parse(pdate.Substring(0, 4));
            int month = int.Parse(pdate.Substring(5, 2));
            int day = int.Parse(pdate.Substring(8, 2));
            DateTime date;
            if (from_to == DateFromTo.from)
            {
                date = PersianToEnglishDate(year, month, day, 0, 0, 0, 0);
            }
            else
            {
                date = PersianToEnglishDate(year, month, day, 23, 59, 59, 0);
            }


            return date;
        }
    }
}
