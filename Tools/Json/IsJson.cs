using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {
        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

    }
}
