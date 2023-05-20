using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {

        internal static Newtonsoft.Json.Linq.JObject RemoveEmptyKeys(object obj, bool remove_null, bool remove_default)
        {
            //add [DefaultValue("")] to must be remved properties  
            var serializer = new Newtonsoft.Json.JsonSerializerSettings();
            if (remove_null) serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            if (remove_default) serializer.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, serializer);
            Newtonsoft.Json.Linq.JObject conv = Newtonsoft.Json.Linq.JObject.Parse(json);
            return conv;
        }
        



    }
}
