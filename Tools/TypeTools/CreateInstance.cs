using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SRLCore.Tools
{
    public partial class TypeTools
    {
        public static ClassType CreateInstance<ClassType>()
        {
            return (ClassType)Activator.CreateInstance(typeof(ClassType));
        }
        public static ClassType CreateInstance<ClassType>(params object[] inputs)
        {
            return (ClassType)Activator.CreateInstance(typeof(ClassType), inputs);
        }
        public static object CreateInstance(string className)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(className);
        }
        public static ClassType CreateInstance<ClassType>(string className)
        {
            return (ClassType)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(className);
        }
    }
}
