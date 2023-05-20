using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SRLCore.Tools
{
    public partial class IdentifierTools
    {
        public static string NumberSeperatorNoDigit(long num)
        {
            return String.Format("{0:n0}", num);
        }
    }

}
