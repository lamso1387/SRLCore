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

namespace SRLCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class UserController<Tcontext, TUser, TRole, TUserRole, TAddUserRequest> : CommonEntityRequestController<Tcontext, TUser, TRole, TUserRole, TUser, TAddUserRequest>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
        where TAddUserRequest : AddUserRequest
    {
        protected abstract void EditUserFieldFromRequest(TUser existing_entity, TUser new_entity);
        protected virtual object UserSessionData(TUser user)
        {
            return new List<TUser> { user }.Select(x => new { x.id, x.first_name, x.last_name, x.create_date, x.full_name, x.mobile }).First();
        }

        public UserController(IDistributedCache distributedCache,
            ILogger<UserController<Tcontext, TUser, TRole, TUserRole, TAddUserRequest>> logger, Tcontext dbContext,
            SRLCore.Services.UserService<Tcontext, TUser, TRole, TUserRole> userService)
             : base(distributedCache, logger, dbContext, userService)
        {

        }

        [HttpPost("authenticatepost")]
        [DisplayName("احراز هویت")]
        public virtual async Task<IActionResult> AuthenticatePost([FromBody] TUser user)
        {
            var response = new SingleResponse<object>();
            user = await _userService.Authenticate(user?.username, user?.password);

            if (user == null)
                throw new Middleware.GlobalException(ErrorCode.Unauthorized);

            List<string> user_accesses = UserSession.GetAccesses(Db, user.id);


            var user_data = UserSessionData(user);

            Dictionary<string, object> Session = new Dictionary<string, object>
            {
                ["Id"] = user.id,
                ["Accesses"] = user_accesses,
                ["UserData"] = user_data
            };

            return response.ToResponse(Session);
        }



        [HttpGet("authenticate")]
        [DisplayName("احراز هویت")]
        public async Task<IActionResult> Authenticate()
        {
            string method = nameof(Authenticate);
            LogHandler.LogMethod(EventType.Call, Logger, method);
            PagedResponse<object> response = new PagedResponse<object>();
            try
            {
                //  response.Model = SRLCore.Model.UserSession<User>.Accesses;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }

            return response.ToHttpResponse(Logger, Request.HttpContext);

        }

        [HttpPost("search")]
        [DisplayName("جستجوی کاربر")]
        public async Task<IActionResult> SearchUser([FromBody] SearchUserRequest request)
        {
            PagedResponse<object> response = new PagedResponse<object>();


            var query = await Db.GetUsers(request).Paging(response, request.page_start, request.page_size)
                .ToListAsync();


            return response.ToResponse(query, x => new
            {
                x.id,
                x.create_date,
                x.creator_id,
                x.first_name,
                x.last_name,
                x.username,
                x.mobile,
                x.status,
                x.full_name
            });

        }

        [HttpPost("add")]
        [DisplayName("افزودن کاربر")]
        public async Task<IActionResult> AddUser([FromBody] TAddUserRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.pass_mode = PassMode.add;
            request.CheckValidation();

            var user = RequestToEntity(request);

            var existingEntity = await Db.GetUser(user.id, null);
            if (existingEntity != null)
            {
                response.ErrorCode = (int)ErrorCode.AddRepeatedEntity;
                return response.ToHttpResponse(Logger, Request.HttpContext);
            }

            user.UpdatePasswordHash();

            await Db.AddAsync(user);
            int save = await Db.SaveChangesAsync();

            if (save == 0)
            {
                response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                return response.ToHttpResponse(Logger, Request.HttpContext);
            }
            var entity_list = new List<TUser> { user }
                .Select(user.Selector).First();
            response.Model = entity_list;
            response.ErrorCode = (int)ErrorCode.OK;


            return response.ToHttpResponse(Logger, Request.HttpContext);
        }

        [DisplayName("حذف کاربر")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            string method = nameof(DeleteUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, id);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {
                var existingEntity = await Db.GetUser(id, null);
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }

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

        [DisplayName("ویرایش کاربر")]
        [HttpPut("edit")]
        public async Task<IActionResult> EditUser([FromBody] TAddUserRequest request)
        {
            string method = nameof(EditUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.CheckValidation();

                var entity = RequestToEntity(request);
                entity.id = (long)request.id;

                var existingEntity = await Db.GetUser(entity.id, entity.username);
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }

                EditUserFieldFromRequest(existingEntity, entity);
                //existingEntity.first_name = entity.first_name;
                //existingEntity.last_name = entity.last_name;
                //existingEntity.mobile = entity.mobile; 
                //existingEntity.password = entity.password;

                existingEntity.UpdatePasswordHash();

                int save = await Db.SaveChangesAsync();
                if (save == 0)
                {
                    response.ErrorCode = (int)ErrorCode.DbSaveNotDone;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }
                var entity_list = new List<TUser> { entity }
                    .Select(entity.Selector).First();
                response.Model = entity_list;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }
            return response.ToHttpResponse(Logger, Request.HttpContext);
        }

        [DisplayName("تغییر رمز توسط  کاربر")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePasswordUser([FromBody] TAddUserRequest request)
        {
            SingleResponse<object> response = new SingleResponse<object>();
              
            var entity = RequestToEntity(request);
            entity.id = user_session_id;

            var existingEntity = await Db.GetUser(entity.id);
            existingEntity.ThrowIfNotExist();

            existingEntity.password = entity.password;
              
            existingEntity.UpdatePasswordHash();

            int save = await Db.UpdateSave(existingEntity, user_session_id); 

            return response.ToResponse(entity,entity.Selector);
        }

        [HttpGet("{id}")]
        [DisplayName("مشاهده کاربر")]
        public async Task<IActionResult> GetUser(long id)
        {
            SingleResponse<object> response = new SingleResponse<object>();
            var existingEntity = await Db.GetUser(id, null);
            existingEntity.ThrowIfNotExist();

            var entity = new List<TUser> { existingEntity }
                .Select(existingEntity.Selector).First();


            return response.ToResponse(existingEntity);
        }

    }
}