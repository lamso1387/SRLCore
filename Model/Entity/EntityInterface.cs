
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

    public interface ICommonProperty
    {
        long id { get; set; }
        long creator_id { get; set; }
        long? modifier_id { get; set; }
        DateTime create_date { get; set; }
        DateTime? modify_date { get; set; }
        string status { get; set; }

    }
    public interface IUserRole<TUser, TRole> : ICommonProperty
    {
        long user_id { get; set; }
        TUser user { get; set; }
        long role_id { get; set; }
        TRole role { get; set; }

    }
    public interface IRole<TUserRole> : ICommonProperty
    {
        ICollection<TUserRole> user_roles { get; set; }
        string name { get; set; }
        string accesses { get; set; }

    }
    public interface IUserProp : ICommonProperty
    {

        string username { get; set; }
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

    public interface IUser<TUserRole> : IUserProp
    {
        ICollection<TUserRole> user_roles { get; set; }
    }

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
    public interface IBaseInfo<TBaseKind> : ICommonProperty
    {
        TBaseKind kind { get; set; }
        string title { get; set; }
        bool? is_default { get; set; }
    }
    public interface ICity<TProvince> : ICommonProperty
    {
        long province_id { get; set; }
        TProvince province { get; set; }
        string title { get; set; }
        string province_title { get; }

    }
    public interface IProvince<TCity> : ICommonProperty
    {
        ICollection<TCity> cities { get; set; }
        string title { get; set; }
    }
}
