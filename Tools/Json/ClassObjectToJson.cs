using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {
        public static string ClassObjectToJson(object obj, Newtonsoft.Json.Formatting format)
        {
            if (obj == null) return null;
            try
            {
                if (obj.GetType() == typeof(string)) obj = Newtonsoft.Json.JsonConvert.DeserializeObject(obj.ToString());

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, format);
                return json;
            }
            catch (Exception ex)
            {

                string json = new JavaScriptSerializer().Serialize(obj);
                return json;
            }



        }
        public static string ClassObjectToJson(object obj)
        {
            return ClassObjectToJson(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
