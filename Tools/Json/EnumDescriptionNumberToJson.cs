using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {

        public static string EnumDescriptionNumberToJson(Type enum_type)
        {
            var ret = "{";
            int first = 0;
            foreach (var val in Enum.GetValues(enum_type))
            {
                if (first != 0) ret += ",";
                var name = Enum.GetName(enum_type, val);


                string enum_des_str = Tools.EnumTools.GetEnumDescription(val);

                ret += $"\n \"{ (int)val}\" : \"{enum_des_str}\"";
                first++;
            }
            ret += " \n }";
            return ret;

        }

       




    }
}
