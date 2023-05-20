using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools 
    { 
        public static T StringToJson<T>(string input) where T : new()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
        }
       
    }
}
