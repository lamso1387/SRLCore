using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;  
using System.Linq; 

namespace SRLCore.Model
{

    /// <summary>
    /// for static instance add this to driven class:   public static IUserSession<Tstatus> Instance => new UserSession<Tstatus>();
    /// </summary> 
    public interface IUserSession<TUser>
    {
        long Id { get; set; }
        List<string> Accesses { get; set; }
        TUser UserData { get; set; }
        void SetAccesses(DbContext _context);
        /// <summary>
        /// return _context.UserRoles.Where(x => x.user_id == Id).Include(x => x.role).Select(x => x.role.accesses).ToList();
        /// </summary>
        GetAllAccess get_all_access { get; }
        Func<TUser, object> SessionFields { get; }
        Dictionary<string, object> Session { get; }

    }

    public abstract class UserSession<TUser> : IUserSession<TUser>
    {
        public virtual long Id { get; set; }
        public virtual List<string> Accesses { get; set; }
        public virtual TUser UserData { get; set; }
        public virtual GetAllAccess get_all_access { get; }
        /// <summary>
        ///  => x => new { x.id, x.first_name, x.last_name, x.create_date, x.full_name, x.mobile };
        /// </summary>
        public abstract Func<TUser, object> SessionFields { get; }
        /// <summary>
        /// => new Dictionary<string, object>{[nameof(Id)] = Id,[nameof(Accesses)] = Accesses,[nameof(UserData)] = new List<TUser> { UserData}.Select(x => SessionFields(x)).First()};
        /// </summary>
        public abstract Dictionary<string, object> Session { get; }
        /// <summary>
        /// set Accesses property
        /// </summary>
        /// <param name="_context"> override with app db context</param>
        public virtual void SetAccesses(DbContext _context)
        {
            List<string> all_access = get_all_access(_context, Id);
            List<string> accesses = new List<string>();
            all_access.ForEach(x => accesses.AddRange(x.Split(",").ToList()));
            Accesses = accesses.Distinct().ToList();
        }
        /// <summary>
        ///    public static new SRLCore.Model.UserSession<User> user_session => new AppUserSession();
        /// </summary>
        public static UserSession<TUser> user_session { get; }

    }

}
