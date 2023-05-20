using System;
using System.Collections.Generic;

namespace SRLCore.Tools
{
    public partial class ConvertorTools
    {
        public static Dictionary<string, string> EnumToDictionary(Type enum_type)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var val in Enum.GetValues(enum_type))
            {
                string enum_des_str = Tools.EnumTools.GetEnumDescription(val);
                dic.Add(val.ToString(), enum_des_str);
            }
            return dic;

        }
    }
}
