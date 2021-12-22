
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
    public interface IUserRole : ICommonProperty
    {
        long user_id { get; set; }
        long role_id { get; set; }

    }
    public interface IRole : ICommonProperty
    {
        string name { get; set; }
        string accesses { get; set; }

    }
    public interface IUser : ICommonProperty
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
        bool? change_pass_next_login { get; set; }
        DateTime? last_login { get; set; }



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
    public interface IProvince : ICommonProperty
    { 
        string title { get; set; }
    }
}
