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
    public abstract class DbEntity<TDb> : DbContext, IDbContext where TDb : DbContext
    {
        public DbEntity(DbContextOptions<TDb> options)
         : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
        /// <summary>
        /// (builder.Property(e => e.mobile), 11)
        /// </summary>
        public static void ColumnFixedLenght<Tprop>(PropertyBuilder<Tprop> pb, int length) => pb.HasMaxLength(length).IsFixedLength();
        /// <summary>
        /// (builder.HasIndex(p => p.username))
        /// </summary>
        public static void ColumnUnique(IndexBuilder ib) => ib.IsUnique();


        public virtual void RestrinctDeleteBehavior(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    } 
    public abstract class CommonProperty<TUser> : ICommonProperty
    {
        [Key]
        public virtual long id { get; set; }
        public virtual long creator_id { get; set; } = UserSession<TUser>.user_session.Id;
        public virtual long? modifier_id { get; set; }
        public virtual DateTime create_date { get; set; } = DateTime.Now;
        public virtual DateTime? modify_date { get; set; }
        public virtual string status { get; set; }
    } 

    public abstract class CommonPropertyConfigurable<TEntity, TUser> : CommonProperty<TUser>
    where TEntity : class
    {
        /// <summary>
        /// eg: public void Configure(EntityTypeBuilder<User> builder){
        /// </summary> 
        public static Action<EntityTypeBuilder<TEntity>> config { get; }

        public class EntityConfiguration : IEntityTypeConfiguration<TEntity>
        {
            Action<EntityTypeBuilder<TEntity>> configuration;
            public EntityConfiguration(Action<EntityTypeBuilder<TEntity>> config_)
            {
                configuration = config_;
            }
            public void Configure(EntityTypeBuilder<TEntity> builder)
            {
                configuration(builder);
            }
        }
    }

    public abstract class Role<TUserRole, TUser> : CommonPropertyConfigurable<Role<TUserRole, TUser>, TUser>, IRole<TUserRole>
    {
        public virtual ICollection<TUserRole> user_roles { get; set; }
        [Required]
        public virtual string name { get; set; }
        [Required]
        public virtual string accesses { get; set; }
        public static new Action<EntityTypeBuilder<Role<TUserRole, TUser>>> config => builder =>
            DbEntity<DbContext>.ColumnUnique(builder.HasIndex(e => e.name));
    }

    public abstract class UserRole<TUser, TRole> : CommonProperty<TUser>, IUserRole<TUser, TRole>
    {
        public virtual long user_id { get; set; }
        public virtual TUser user { get; set; }
        public virtual long role_id { get; set; }
        public virtual TRole role { get; set; }
    }
    public abstract class User<TUserRole, TUser> : CommonPropertyConfigurable<User<TUserRole, TUser>, TUser>, IUser<TUserRole>
    {

        public virtual ICollection<TUserRole> user_roles { get; set; }
        [Required]
        public virtual string username { get; set; }
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

        public static new Action<EntityTypeBuilder<User<TUserRole, TUser>>> config => builder =>
         DbEntity<DbContext>.ColumnFixedLenght(builder.Property(e => e.mobile), 11);
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
        public abstract Dictionary<string, object> Session { get;  }
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
        public static UserSession<TUser> user_session { get; }

    }

    public abstract class BaseInfo<BaseKind, TUser> : CommonProperty<TUser>, IBaseInfo<BaseKind>
    {
        [Required]
        public virtual BaseKind kind { get; set; }
        [Required]
        public virtual string title { get; set; }
        public virtual bool? is_default { get; set; }
    }
    public abstract class City<TProvince, TUser> : CommonProperty<TUser>, ICity<TProvince> where TProvince : IProvince<TProvince>
    {
        public virtual long province_id { get; set; }
        public virtual TProvince province { get; set; }
        [Required]
        public virtual string title { get; set; }
        [NotMapped]
        public virtual string province_title => province?.title;
    }
    public abstract class Province<TCity, TUser> : CommonProperty<TUser>, IProvince<TCity>
    {
        public virtual ICollection<TCity> cities { get; set; }
        [Required]
        public virtual string title { get; set; }
    }



}

