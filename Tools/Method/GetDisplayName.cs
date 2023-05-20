using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRLCore.Tools
{
    public partial class MethodTools
    {
        public static string GetDisplayName(MethodInfo method)
        {
            string name = method.CustomAttributes
                       .Where(y => y.AttributeType == typeof(DisplayNameAttribute))?.First()
                       ?.ConstructorArguments?.First().Value?.ToString();
            return name;
        } 
    }
}
