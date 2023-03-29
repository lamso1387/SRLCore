using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model.Constants
{
    public class SqlConstarint
    {
        public static string PersianDateString(string column_name) =>
        $"([{column_name}] like '1[34][0-9][0-9]/_____' AND([{column_name}] like '_____1[0-2]/[12][0-9]' OR[{column_name}] like '_____0[1-9]/[12][0-9]' OR[{column_name}] like '_____1[0-2]/3[01]' OR[{column_name}] like '_____0[1-9]/3[01]' OR[{column_name}] like '_____1[0-2]/0[1-9]' OR[{column_name}] like '_____0[1-9]/0[1-9]'))";
        public static string OfficialBarnameNoConstraint="'14[0-9][0-9]/[0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]'";
        public static string OfficialBarnameNo(string column_name) => $"([{column_name}] like {OfficialBarnameNoConstraint}";
    }
}
