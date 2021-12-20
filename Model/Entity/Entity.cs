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
        public abstract void ModelCreator(ModelBuilder modelBuilder);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreator(modelBuilder); 
            base.OnModelCreating(modelBuilder);
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
    /// <summary>
    /// implement  creator_id = UserSession<TUser>.user_session.Id;
    /// </summary> 
    public abstract class CommonProperty : ICommonProperty
    {
        [Key]
        public virtual long id { get; set; }
        public abstract long creator_id { get; set; }
        public virtual long? modifier_id { get; set; }
        public virtual DateTime create_date { get; set; } = DateTime.Now;
        public virtual DateTime? modify_date { get; set; }
        [Required]
        public virtual string status { get; set; } = EntityStatus.active.ToString();
    } 

    public abstract class CommonPropertyConfigurable<TEntity> : CommonProperty
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

 


}

