using SRLCore.Middleware;
using SRLCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json; 
using System.ComponentModel.DataAnnotations.Schema;
using System.IO; 


namespace SRLCore.Model
{
     public class RoleConstraint
    {
        public string column_name { get; set; }
        public List<string> constraints { get; set; } 

    }
}
