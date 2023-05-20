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

        public static MethodInfo[] GetPublicMethods(Type class_type)
        {
            MethodInfo[] methods = class_type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return methods;
        }
    }
}
