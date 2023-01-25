using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Http;
using SRLCore.Middleware;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace SRLCore.Model
{
    public abstract class DbEntity<TDb> : DbContext, IDbContext where TDb : DbContext
    {

        public DbEntity(DbContextOptions<TDb> options)
         : base(options)
        {

        }
        public int ExecuteTruncateTable(string table_name)
        {
            string query = $"TRUNCATE TABLE {table_name};";

            return ExecuteNonSqlQuery(query);

        }
        public int ExecuteNonSqlQuery(string query, int? time_out_second = null)
        {

            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if (time_out_second != null)
                    command.CommandTimeout = (int)time_out_second;

                Database.OpenConnection();

                return command.ExecuteNonQuery();

            }

        }
        public SqlDataReader ExecuteDataQuery(string query)
        {

            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                Database.OpenConnection();

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();

                return dr;

            }

        }
        public int ExecuteCount(string query)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                Database.OpenConnection();
                var count_ = command.ExecuteScalar();
                int count_2 = (int)count_;
                Database.CloseConnection();
                return count_2;
            }

        }

        public abstract string GetConnectionString();

        public virtual TDb GetNewDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDb>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            return SRL.ClassManagement.CreateInstance<TDb>(optionsBuilder.Options);
        }

        public abstract void ModelCreator(ModelBuilder modelBuilder);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreator(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        public virtual void Configuration(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            Configuration(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
        /// <summary>
        /// (builder.Property(e => e.mobile), 11)
        /// </summary>
        public void ColumnFixedLenght<Tprop>(PropertyBuilder<Tprop> pb, int length) => pb.HasMaxLength(length).IsFixedLength();
        /// <summary>
        /// (builder.HasIndex(p => p.username))
        /// </summary>
        public void ColumnUnique(IndexBuilder ib) => ib.IsUnique();


        public string GetTableName<T>() where T : class
        {
            var entityTypes = Model.GetEntityTypes();

            var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
            var TableName = tableNameAnnotation.Value.ToString();
            return TableName;
        }

        public virtual void RestrinctDeleteBehavior(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public async Task AddSave<T>(T entity)
        {
            await AddAsync(entity);
            int save = await SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        public async Task<int> AddRangeSave<T>(List<T> entity_list) where T : class
        {
            //Set<T>().AddRange(request);
            //SaveChanges();

            await Set<T>().AddRangeAsync(entity_list);
            int save = await SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);
            else return save;
        }
        public async Task RemoveSave<T>(T entity)
        {
            Remove(entity);
            int save = await SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        public async Task<int> UpdateSave(bool throw_if_not_saved = true)
        {
            int save = await SaveChangesAsync();
            if (save == 0 && throw_if_not_saved) throw new GlobalException(ErrorCode.DbSaveNotDone);
            else return save;

        }
        public async Task Save()
        {
            int save = await SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }

    }
    public abstract class DbEntity<TDb, TUser, TRole, TUserRole> : DbEntity<TDb>, IDbContext<TUser, TRole, TUserRole>
        where TDb : DbContext
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
    {
        public virtual DbSet<TUser> Users { get; set; }
        public virtual DbSet<TRole> Roles { get; set; }
        public virtual DbSet<TUserRole> UserRoles { get; set; }

        public DbEntity(DbContextOptions<TDb> options)
         : base(options)
        {

        }

        public virtual IQueryable<TUser> GetUsers(long? id = null)
        {
            var query = Users.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);


            return query;
        }
        public virtual IQueryable<TUser> GetUsers(SearchUserRequest request)
        {
            return GetUsers(request.id);

        }
        public virtual async Task<TUser> GetUser(long id, string username=null)
        => await Users.FirstOrDefaultAsync(item => item.id == id || (item.username == username));


        public virtual async Task<TRole> GetRole(long? id, string name = null)
   => await Roles.FirstOrDefaultAsync(item => item.id == id || item.name == name);
        public virtual IQueryable<TRole> GetRoles(long? id = null, string name = null)
        {
            var query = Roles.AsQueryable();

            if (id.HasValue)
                query = query.Where(item => item.id == id);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(item => item.name == name);

            return query;
        }
        public virtual async Task<TUserRole> GetUserRole(long? id, long? user_id = null, long? role_id = null)
=> await UserRoles.FirstOrDefaultAsync(item => item.id == id || (item.user_id == user_id && item.role_id == role_id));

        public virtual IQueryable<TUserRole> GetUserRoles(long role_id)
=> UserRoles.Where(item => item.role_id == role_id).AsQueryable();


    }
    /// <summary>
    /// implement  creator_id = UserSession<TUser>.user_session.Id;
    /// </summary> 
    public abstract class CommonProperty : ICommonProperty
    {
        [Key]
        public virtual long id { get; set; }
        public virtual long creator_id { get; set; }
        public virtual long? modifier_id { get; set; }
        public virtual DateTime create_date { get; set; } = DateTime.Now;
        public virtual DateTime? modify_date { get; set; }
        [Required]
        public virtual string status { get; set; } = EntityStatus.active.ToString();
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public virtual Func<CommonProperty, object> Selector => x => new
        {
            x.id,
            x.create_date,
            x.creator_id,
            x.modifier_id,
            x.modify_date
        };

        [NotMapped]
        public virtual EntityStatus status_enum
        {
            get => SRL.Convertor.StringToEnum<EntityStatus>(status);
            set { status = value.ToString(); }
        }

    }


    public abstract class IUserRole : CommonProperty
    {
        public abstract long user_id { get; set; }
        public abstract long role_id { get; set; }

    }

    public abstract class IRole : CommonProperty
    {
        public abstract string name { get; set; }
        public abstract string accesses { get; set; }

    }
    public abstract class IUser : CommonProperty
    {
        public abstract string username { get; set; }
        public abstract string first_name { get; set; }
        public abstract string last_name { get; set; }
        public abstract string mobile { get; set; }
        public abstract byte[] password_hash { get; set; }
        public abstract string password { get; set; }
        public abstract byte[] password_salt { get; set; }
        public abstract string full_name { get; }
        public abstract bool? change_pass_next_login { get; set; }
        public abstract DateTime? last_login { get; set; }
        [NotMapped]
        public string[] columns_seed { get; set; } =
         new string[] { nameof(creator_id), nameof(create_date), nameof(status), nameof(first_name), nameof(last_name), nameof(mobile), nameof(password_hash), nameof(password_salt) };


        public void SetAdminSeed(string pass = null)
        {
            creator_id = 1;
            create_date = DateTime.Now;
            status = EntityStatus.active.ToString();
            first_name = "admin";
            last_name = "admin";
            mobile = "09000000000";
            if (pass == null) password = "12345678";
            else password = pass;
            UpdatePasswordHash();
        }
        public static void CreatePasswordHashS(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public void UpdatePasswordHash()
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHashS(password, out byte[] passwordHash, out byte[] passwordSalt);
                password_hash = passwordHash;
                password_salt = passwordSalt;
            }
        }
    }

    public abstract class IProvince : CommonProperty
    {
        public abstract string title { get; set; }
    }


    public static class EntityExtensions
    {
        public static void ThrowIfNotExist(this CommonProperty existingEntity)
        { if (existingEntity == null) throw new GlobalException(SRLCore.Model.ErrorCode.ItemNotExists); }



    }

}

