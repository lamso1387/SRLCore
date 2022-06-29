using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel;
using SRL;
using SRLCore.Model;
using System.Reflection;
using SRLCore.Middleware;
using System.Linq.Dynamic.Core;

namespace SRLCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class RoleController<Tcontext, TUser, TRole, TUserRole> : CommonController<Tcontext, TUser, TRole, TUserRole>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
    {
        protected abstract TRole RequestToEntity(AddRoleRequest requst, long? edit_id = null);
        protected abstract TUserRole CreateUserRole(TRole role, TUser user);
        protected virtual string user_roles_col_name =>"user_roles";
        protected virtual string user_col_name => "user";


        protected virtual List<TUser> LoadUserRoles(TRole existingEntity)
        {
            Db.Entry(existingEntity).Collection<TUserRole>(user_roles_col_name).Query().Include(user_col_name).Load();
            var users = ((ICollection<TUserRole>)SRL.ClassManagement.GetProperty(user_roles_col_name, existingEntity)).AsQueryable().Select(user_col_name).ToDynamicList<TUser>();
            return users;
        }

        protected virtual   Func<TRole, object> RoleSelector(List<TUser> users)
        {
            var users_select = users.Select(y => new { y.first_name, y.last_name, y.full_name, y.id }).ToArray();
            var users_ids = users.Select(y => y.id);

            return x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                accesses = x.accesses.Split(","),
                x.name,
                x.status,
                users = users_select,
                user_ids = users_ids
            };
        }

        protected abstract System.Linq.Expressions.Expression<Func<TUserRole, TUser>> UserSelector { get; } 

        protected abstract Func<TUserRole, TUser> UserRoleSelector { get; } //=> x => null;
        protected abstract Assembly GetCurrentAssembly { get; }
        //Assembly.GetAssembly(typeof(CommonController<Tcontext, TUser, TRole, TUserRole>))

        public RoleController(IDistributedCache distributedCache,
        ILogger<RoleController<Tcontext, TUser, TRole, TUserRole>> logger, Tcontext dbContext,
        SRLCore.Services.UserService<Tcontext, TUser, TRole, TUserRole> userService)
         : base(distributedCache, logger, dbContext, userService)
        {

        }

        [HttpPost("search")]
        [DisplayName("جستجوی نقش")]
        public async Task<IActionResult> SearchRole()
        {
            PagedResponse<object> response = new PagedResponse<object>();

            var query = await Db.GetRoles().Paging(response).ToListAsync();

            Func<TRole, object> selector = x => new
            {
                x.id,
                x.name,
            };

            return response.ToResponse(query, selector);
        }

        [HttpPost("add")]
        [DisplayName("افزودن نقش")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequest request)
        {

            SingleResponse<object> response = new SingleResponse<object>();


            request.CheckValidation(response);

            var entiry = RequestToEntity(request);

            var existingEntity = await Db.GetRole(null, entiry.name);
            if (existingEntity != null)
            {
                response.ErrorCode = (int)ErrorCode.AddRepeatedEntity;
                return response.ToHttpResponse(Logger, Request.HttpContext);
            }
            foreach (var userId in request.user_ids)
            {
                var user = await Db.GetUser(userId, null);
                var user_role = CreateUserRole(entiry, user);
                await Db.AddAsync(user_role);
            }


            int save = await Db.SaveChangesAsync();
            if (save == 0)
            {
                response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                return response.ToHttpResponse(Logger, Request.HttpContext);
            }
            var entity_list = new List<TRole> { entiry }
                .Select(x => new
                {
                    x.id,
                    x.create_date,
                    x.creator_id,
                    x.name,
                    x.status,
                    x.accesses,
                    // request.user_ids
                }).First();

            return response.ToResponse(entity_list, x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                x.name,
                x.status,
                x.accesses,
                // request.user_ids
            });
        }

        [HttpDelete("delete/{id}")]
        [DisplayName("حذف نقش")]
        public async Task<IActionResult> DeleteRole(long id)
        {
            string method = nameof(DeleteRole);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {
                var existingEntity = await Db.GetRole(id);
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }

                var existingUserRoles = Db.GetUserRoles(existingEntity.id);

                Db.RemoveRange(existingUserRoles);
                Db.Remove(existingEntity);
                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }
                response.Model = true;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger, Request.HttpContext);
        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده نقش")]
        public async Task<IActionResult> GetRole(long id)
        {

            SingleResponse<object> response = new SingleResponse<object>();

            var existingEntity = await Db.GetRole(id);
            existingEntity.ThrowIfNotExist();

            List<TUser> users = LoadUserRoles(existingEntity);

            return response.ToResponse(existingEntity, RoleSelector(users));
        }

        [HttpPut("edit")]
        [DisplayName("ویرایش نقش")]
        public async Task<IActionResult> EditRole([FromBody] AddRoleRequest request)
        {

            SingleResponse<object> response = new SingleResponse<object>();
            request.CheckValidation(response);

            var entiry = RequestToEntity(request, request.id);

            var existingEntity = await Db.GetRole(entiry.id);
            existingEntity.ThrowIfNotExist();

            if (existingEntity.name != entiry.name)
            {
                var existingName = await Db.GetRole(null, entiry.name);
                if (existingName != null)

                    throw new GlobalException(SRLCore.Model.ErrorCode.AddRepeatedEntity);


            }
            existingEntity.name = entiry.name;
            existingEntity.modifier_id = user_session_id;
            existingEntity.modify_date = DateTime.Now;
            existingEntity.accesses = entiry.accesses;
             

            IEnumerable<TUser> users_old = Db.UserRoles.Where(x => x.role_id == existingEntity.id).Select(UserSelector);
            var user_ids_to_delet = users_old.Select(x => x.id).Where(x => !request.user_ids.Select(y => y).Contains(x));
            var user_ids_new = request.user_ids.Select(x => x).Where(x => !users_old.Select(y => y.id).Contains(x));

            


            foreach (var user_id_to_delet in user_ids_to_delet)
            {
                var user_role_delet = await Db.GetUserRole(null, user_id_to_delet, existingEntity.id);
                Db.Remove(user_role_delet);
            }
            foreach (var user_new_id in user_ids_new)
            {
                var new_user = await Db.GetUser(user_new_id, null);
                var user_role = CreateUserRole(existingEntity, new_user);

                await Db.AddAsync(user_role);
            }


            await Db.Save();
            var entity_list = new List<TRole> { entiry }
                .Select(x => new
                {
                    x.id,
                    x.create_date,
                    x.creator_id,
                    x.name,
                    x.status,
                    x.accesses
                    //userids
                }).First();

            return response.ToResponse(entity_list);

        }

        [HttpGet("accesses")]
        [DisplayName("مشاهده دسترسی ها")]
        public async Task<IActionResult> GetAccesses()
        {
            SingleResponse<object> response = new SingleResponse<object>();

            IEnumerable<Type> all_controller_types = SRL.ChildParent
                .GetAllChildrenClasses<CommonController<Tcontext, TUser, TRole, TUserRole>>(GetCurrentAssembly);

            List<object> action_titles = new List<object>();
            foreach (var controller_type in all_controller_types)
            {

                MethodInfo[] actions = SRL.ActionManagement.Method.GetPublicMethods(controller_type);
                if (actions.Any())
                {
                    var titles = actions.Select(x => new
                    {
                        name = x.Name,
                        title = SRL.ActionManagement.Method.GetDisplayName(x)
                    });
                    action_titles.AddRange(titles);
                }
            }

            return response.ToResponse(action_titles);
        }



    }
}