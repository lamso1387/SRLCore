
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace SRLCore.Model
{
    public interface IDbContext
    {
        void RestrinctDeleteBehavior(ModelBuilder modelBuilder);

    }
    public interface ICommonProperty<Tstatus>
    {
        long id { get; set; }
        long creator_id { get; set; }
        long? modifier_id { get; set; }
        DateTime create_date { get; set; }
        DateTime? modify_date { get; set; }
        Tstatus status { get; set; }

    }
    public interface ICommonProperty : ICommonProperty<string> { }
    public interface IUserRole<Tstatus> : ICommonProperty<Tstatus>
    {
        long user_id { get; set; }
        IUser<Tstatus> user { get; set; }
        long role_id { get; set; }
        IRole<Tstatus> role { get; set; }

    }
    public interface IUserRole : IUserRole<string> { }
    public interface IRole<Tstatus> : ICommonProperty<Tstatus>
    {
        ICollection<IUserRole<Tstatus>> user_roles { get; set; }
        string name { get; set; }
        string accesses { get; set; }

    }
    public interface IRole : IRole<string> { }
    public interface IUser<Tstatus> : ICommonProperty<Tstatus>
    {

        ICollection<IUserRole<Tstatus>> user_roles { get; set; }
        string first_name { get; set; }
        string last_name { get; set; }
        string mobile { get; set; }
        byte[] password_hash { get; set; }
        string password { get; set; }
        byte[] password_salt { get; set; }
        /// <summary>
        ///implementation: { get => $"{first_name} {last_name}"; }
        /// </summary>
        string full_name { get; }

    }
    public interface IUser : IUser<string> { }

    /// <summary>
    /// for static instance add this to driven class:   public static IUserSession<Tstatus> Instance => new UserSession<Tstatus>();
    /// </summary> 
    public interface IUserSession<Tstatus>
    {
        long Id { get; set; }
        List<string> Accesses { get; set; }
        IUser<Tstatus> UserData { get; set; }
        void SetAccesses(DbContext _context);
        /// <summary>
        /// return _context.UserRoles.Where(x => x.user_id == Id).Include(x => x.role).Select(x => x.role.accesses).ToList();
        /// </summary>
        GetAllAccess get_all_access { get; }
        Func<IUser<Tstatus>, object> SessionFields { get;  }
        Dictionary<string, object> Session { get; } 

    }
    public interface IUserSession : IUserSession<string> { }
    public interface IBaseInfo<TBaseKind, Tstatus> :  ICommonProperty<Tstatus>
    {
        TBaseKind kind { get; set; } 
        string title { get; set; }
        bool? is_default { get; set; }   
    }
    public interface IBaseInfo<TBaseKind> : IBaseInfo<TBaseKind,string> { }
    public interface ICity<Tstatus> : ICommonProperty<Tstatus>
    {
        long province_id { get; set; }
        IProvince<Tstatus> province { get; set; } 
        string title { get; set; }
        string province_title { get; }

    }
    public interface ICity : ICity<string> { }
    public interface IProvince<Tstatus> : ICommonProperty<Tstatus>
    {
        ICollection<ICity<Tstatus>> cities { get; set; } 
         string title { get; set; }
    }
    public interface IProvince : IProvince<string> { }
}
