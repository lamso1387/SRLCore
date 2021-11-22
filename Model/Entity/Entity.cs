using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SRLCore.Model
{
    public class SrlDbContext<TDb> : DbContext, IDbContext where TDb : DbContext
    {
        public SrlDbContext(DbContextOptions<TDb> options)
          : base(options)
        {

        }
        public virtual void RestrinctDeleteBehavior(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;//.OnDelete(DeleteBehavior.Cascade);
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
    public abstract class CommonProperty<Tstatus> : ICommonProperty<Tstatus>
    {
        [Key]
        public virtual long id { get; set; }
        public virtual long creator_id { get; set; } = UserSession.user_session.Id;
        public virtual long? modifier_id { get; set; }
        public virtual DateTime create_date { get; set; } = DateTime.Now;
        public virtual DateTime? modify_date { get; set; }
        public virtual Tstatus status { get; set; }
    }
    public abstract class CommonProperty : CommonProperty<string> { }
    public class Role<Tstatus> : CommonProperty<Tstatus>, IRole<Tstatus>

    {
        public virtual ICollection<IUserRole<Tstatus>> user_roles { get; set; }
        [Required]
        public virtual string name { get; set; }
        [Required]
        public virtual string accesses { get; set; }
    }
    public class Role : Role<string> { }

    public class UserRole<Tstatus> : CommonProperty<Tstatus>, IUserRole<Tstatus>
    {
        public virtual long user_id { get; set; }
        public virtual IUser<Tstatus> user { get; set; }
        public virtual long role_id { get; set; }
        public virtual IRole<Tstatus> role { get; set; }
    }
    public class UserRole : UserRole<string> { }
    public class User<Tstatus> : CommonProperty<Tstatus>, IUser<Tstatus>
    {

        public virtual ICollection<IUserRole<Tstatus>> user_roles { get; set; }
        [Required]
        public virtual string first_name { get; set; }
        [Required]
        public virtual string last_name { get; set; }
        [Required]
        public virtual string mobile { get; set; }
        [Required]
        public virtual byte[] password_hash { get; set; }
        [NotMapped]
        public virtual string password { get; set; }
        [Required]
        public virtual byte[] password_salt { get; set; }
        [NotMapped]
        public string full_name { get => $"{first_name} {last_name}"; }

        /// <summary>
        /// eg: public void Configure(EntityTypeBuilder<User> builder){
        /// </summary>
        [NotMapped]
        public virtual Action<EntityTypeBuilder<User<Tstatus>>> config { get; }

        public class UserConfiguration : IEntityTypeConfiguration<User<Tstatus>>
        {
            User<Tstatus> User;
            public UserConfiguration(User<Tstatus> user_) { User = user_; }
            public void Configure(EntityTypeBuilder<User<Tstatus>> builder)
            {
                Action<EntityTypeBuilder<User<Tstatus>>> user_config = User.config;
                if (user_config != null) User.config(builder);
            }
        }
    }
    public class User : User<string> { }
    public class UserSession<Tstatus> : IUserSession<Tstatus>
    {
        public virtual long Id { get; set; }
        public virtual List<string> Accesses { get; set; }
        public virtual IUser<Tstatus> UserData { get; set; }
        public virtual GetAllAccess get_all_access { get; }
        public virtual Func<IUser<Tstatus>, object> SessionFields =>
            x => new
            {
                x.id,
                x.first_name,
                x.last_name,
                x.create_date,
                x.full_name,
                x.mobile
            };
        public virtual Dictionary<string, object> Session =>
                            new Dictionary<string, object>
                            {
                                [nameof(Id)] = Id,
                                [nameof(Accesses)] = Accesses,
                                [nameof(UserData)] = new List<IUser<Tstatus>> { UserData }
                        .Select(x => SessionFields(x)).First()
                            };

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
        public static IUserSession<Tstatus> user_session => new UserSession<Tstatus>();

    }
    public class UserSession : UserSession<string> { }

    public class BaseInfo<BaseKind, Tstatus> : CommonProperty<Tstatus>, IBaseInfo<BaseKind, Tstatus>
    {
        [Required]
        public virtual BaseKind kind { get; set; }
        [Required]
        public virtual string title { get; set; }
        public virtual bool? is_default { get; set; }
    }
    public class City<Tstatus> : CommonProperty<Tstatus>, ICity<Tstatus>
    {
        public virtual long province_id { get; set; }
        public virtual IProvince<Tstatus> province { get; set; }
        [Required]
        public virtual string title { get; set; }
        [NotMapped]
        public virtual string province_title => province?.title;
    }
    public class Province<Tstatus> : CommonProperty<Tstatus>, IProvince<Tstatus>
    {
        public virtual ICollection<ICity<Tstatus>> cities { get; set; }
        [Required]
        public virtual string title { get; set; }
    }



}

