using System;
using System.Collections.Generic;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static string Shaba(string shaba)
        {
            shaba = (shaba.Length == 24 && shaba.Substring(0, 2).ToLower() != "ir") ? "IR" + shaba : shaba;
            return shaba;

        }
    }
}
