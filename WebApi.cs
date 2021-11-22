//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace SRLCore
//{
//    public class Entity
//    {
//        public class Role : CommonProperty
//        {
//            public ICollection<UserRole> user_roles { get; set; }
//            [Required]
//            public string name { get; set; }
//            [Required]
//            public string accesses { get; set; }
//            [Column(TypeName = "nvarchar(50)")]
//            public EntityStatus status { get; set; }

//        }

//        public class UserRole : CommonProperty
//        {
//            public long user_id { get; set; }
//            public User user { get; set; }
//            public long role_id { get; set; }
//            public Role role { get; set; }

//        }
//        public class User : CommonProperty
//        {

//            public ICollection<UserRole> user_roles { get; set; }
//            public Expert expert { get; set; }

//            [Required]
//            public string national_code { get; set; }
//            [Required]
//            public string first_name { get; set; }
//            [Required]
//            public string last_name { get; set; }
//            [Required]
//            public string mobile { get; set; }
//            [Required]
//            public byte[] password_hash { get; set; }
//            [NotMapped]
//            public string password { get; set; }
//            [Required]
//            public byte[] password_salt { get; set; }
//            [NotMapped, DisplayName("نام و نام خانوادگی")]
//            public string full_name { get => $"{first_name} {last_name}"; }
//            public class UserConfiguration : IEntityTypeConfiguration<User>
//            {
//                public void Configure(EntityTypeBuilder<User> builder)
//                {
//                    builder.Property(e => e.mobile).HasMaxLength(11).IsFixedLength();
//                    builder.Property(e => e.national_code).HasMaxLength(10).IsFixedLength();
//                }
//            }
//        }

//        public class UserSession
//        {
//            public static long Id { get; set; }
//            public static List<string> Accesses { get; set; }
//            public static User UserData { get; set; }

//            public static void SetAccesses(DbContext _context)
//            {
//                List<string> all_access = _context.UserRoles.Where(x => x.user_id == Id)
//         .Include(x => x.role)
//         .Select(x => x.role.accesses).ToList();
//                List<string> accesses = new List<string>();
//                all_access.ForEach(x => accesses.AddRange(x.Split(",").ToList()));
//                Accesses = accesses.Distinct().ToList();

//            }
          
//            public static Dictionary<string, object> Session
//            {
//                get
//                {
//                    return new Dictionary<string, object>
//                    {
//                        [nameof(Id)] = Id,
//                        [nameof(Accesses)] = Accesses,
//                        [nameof(UserData)] = new List<User> { UserData }
//                            .Select(x => new
//                            {
//                                x.id,
//                                x.first_name,
//                                x.last_name,
//                                x.create_date,
//                                x.full_name,
//                                x.mobile,
//                                x.national_code
//                            }).First()
//                    };
//                }

//            }
//        }

//        public abstract class CreationProperty
//        {
//            public long creator_id { get; set; } = UserSession.Id;
//            public long? modifier_id { get; set; }
//            public DateTime create_date { get; set; } = DateTime.Now;
//            public DateTime? modify_date { get; set; }


//        }
//        public abstract class CommonProperty : CreationProperty
//        {
//            [Key]
//            public long id { get; set; }
//        }

//    }
//}
