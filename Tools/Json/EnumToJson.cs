using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {


        public static string EnumToJson(Type enum_type)
        {
            //enum_type=typeof(EnumTypeName)

            var ret = "{";
            int first = 0;
            foreach (var val in Enum.GetValues(enum_type))
            {
                if (first != 0) ret += ",";
                var name = Enum.GetName(enum_type, val);

                ret += $"\n \"{ name}\" : \"{((int)val).ToString()}\"";
                first++;
            }
            ret += " \n }";
            return ret;

        }

     







    }
}
