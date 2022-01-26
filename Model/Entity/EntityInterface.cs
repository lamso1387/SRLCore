
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
        void ModelCreator(ModelBuilder modelBuilder);
        string GetConnectionString();
    }
    public interface IDbContext<TUser, TRole, TUserRole> : IDbContext where TUser : IUser where TRole : class where TUserRole : class
    {
        DbSet<TUser> Users { get; set; }
        DbSet<TRole> Roles { get; set; }
        DbSet<TUserRole> UserRoles { get; set; }
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

    public interface IBaseInfo<TBaseKind> : ICommonProperty
    {
        TBaseKind kind { get; set; }
        string title { get; set; }
        bool? is_default { get; set; }
    }
    public interface ICity : ICommonProperty
    {
        long province_id { get; set; }
        string title { get; set; }
        string province_title { get; }

    }
 
}
