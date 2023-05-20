using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SRLCore.Tools
{
    public partial class TypeTools
    {
        public static object GetProperty<ClassType>(string property_name, ClassType instance)
        {
            PropertyInfo propk = typeof(ClassType).GetProperty(property_name);
            return propk.GetValue(instance);
        }
    }
}
