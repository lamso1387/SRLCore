using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SRLCore.Tools
{
    public partial class EnumTools
    {
        public static string GetEnumDescription<ClassType>(ClassType enum_value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])enum_value.GetType().GetField(enum_value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        public static Array GetEnumValues<Tenum>()=> Enum.GetValues(typeof(Tenum));
        public static string[] GetEnumNames<Tenum>() => Enum.GetNames(typeof(Tenum));
    }
}
