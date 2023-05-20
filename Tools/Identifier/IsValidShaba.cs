using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SRLCore.Tools
{
    public partial class IdentifierTools
    {
        public static bool IsValidShaba(string shaba)
        {
            if (string.IsNullOrWhiteSpace(shaba))
                return false;

            shaba =SRLCore.Tools.ConvertorTools.Shaba(shaba);
            if (shaba.Length != 26)
                return false;

            if (shaba.Substring(0, 2).ToLower() != "ir")
                return false;

            return true;
        }
    }

}
