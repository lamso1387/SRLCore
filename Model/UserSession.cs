using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SRLCore.Model
{

    public abstract class UserSession<TUser> : UserSession where TUser : IUser
    { 
        public static Func<TUser, object> SessionFields
        => x => new { x.id, x.first_name, x.last_name, x.create_date, x.full_name, x.mobile };
         

    }

    public abstract class UserSession
    {  
        public static GetAllAccess get_all_access { get; set; }
        /// <summary>
        /// set Accesses property
        /// </summary>
        /// <param name="_context"> override with app db context</param>
        public static List<string> GetAccesses(DbContext _context, long user_id)
        {
            List<string> all_access = get_all_access(_context, user_id);
            List<string> accesses = new List<string>();
            all_access.ForEach(x => accesses.AddRange(x.Split(",").ToList()));
           return accesses.Distinct().ToList();
        }
    }


}
