using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SRLCore.Model
{

    public abstract class SrlStartup<T> where T : SettingClass
    {
        public virtual string setting_file_name { get; set; } = @"setting.json"; 
        public static T setting;
        public virtual void LoadSetting()
        {
            TextReader tr = new StreamReader(setting_file_name);
            string k = tr.ReadToEnd();
            setting= Newtonsoft.Json.JsonConvert.DeserializeObject<T>(k); 
        }
          

        public static void AppLog(string method, long user_id, string message = "" ,object data = null )
        {
            string log = $"method:{method};user:{user_id}";

            if (!string.IsNullOrWhiteSpace(message))
                log += $"message:{ message};";

            if (data !=null)
                log += $"data:{ Newtonsoft.Json.JsonConvert.SerializeObject(data)};";

            log += $"date:{DateTime.Now}";

            System.IO.File.AppendAllText(setting.log_file_path, Environment.NewLine + log);
        }

    } 
}
