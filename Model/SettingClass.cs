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
        public string db_connection;
        public string log_file_path;
        public long admin_user_id;

    }
}
