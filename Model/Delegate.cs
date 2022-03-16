using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model
{
    /// <summary>
    ///  public GetAllAccess get_all_access_action;
    /// </summary> 
    public delegate List<string> GetAllAccess(DbContext db, long user_id); 

}
