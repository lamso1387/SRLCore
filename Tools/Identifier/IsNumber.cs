using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SRLCore.Tools
{
    public partial class IdentifierTools
    {
        public static bool IsNumber(string str)
        {
            var regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(str))
                return false;
            else return true;
        }
    }

}
