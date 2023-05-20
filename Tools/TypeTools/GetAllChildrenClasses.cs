using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SRLCore.Tools
{
    public partial class TypeTools
    {
        public static List<Type> GetAllChildrenClasses<TParentClass>(Assembly assembly)
        {
            //assembly for asp core: Assembly.GetAssembly(typeof(TParentClass))) 
            List<Type> type_list = new List<Type>();
            var ts = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(TParentClass)));
            type_list.AddRange(ts);


            return type_list;
        }
    }
}
