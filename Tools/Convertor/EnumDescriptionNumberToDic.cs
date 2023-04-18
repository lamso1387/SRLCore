using System;
using System.Collections.Generic;

namespace SRLCore.Tools
{
    public class Convertor
    {
        public static Dictionary<int, string> EnumDescriptionNumberToDic(Type enum_type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var val in Enum.GetValues(enum_type))
            {
                string enum_des_str = SRL.ClassManagement.GetEnumDescription(val);
                dic.Add((int)val, enum_des_str);
            }
            return dic;

        }
    }
}
