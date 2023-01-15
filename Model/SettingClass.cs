using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SRLCore.Model
{
    public abstract class SettingClass 
    {
        public string db_connection { get; set; }
        public string log_file_path { get; set; }
        public long admin_user_id { get; set; }
        public string seeder_username { get; set; }
        public string seeder_password { get; set; }

    }
}
