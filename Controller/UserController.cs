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
    public abstract class UserController<Tcontext, TUser, TRole, TUserRole> : CommonController<Tcontext, TUser, TRole, TUserRole>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
    {
        protected virtual TUser RequestToEntity(AddUserRequest requst) { return null; }
        public static Func<TUser, object> OrderBurberrySelector => x => new
        {
            x.id,
            x.create_date,
            x.creator_id,
            x.status,
            x.first_name,
            x.last_name,
            x.mobile,
            x.username,
        };
        public UserController(IDistributedCache distributedCache,
            ILogger<UserController<Tcontext, TUser, TRole, TUserRole>> logger, Tcontext dbContext,
            SRLCore.Services.UserService<Tcontext, TUser, TRole, TUserRole> userService)
             : base(distributedCache, logger, dbContext, userService)
        {

        }

        [HttpPost("authenticatepost")]
        [DisplayName("احراز هویت")]
        public async Task<IActionResult> AuthenticatePost([FromBody] TUser user)
        {
            string method = nameof(AuthenticatePost);
            LogHandler.LogMethod(EventType.Call, Logger, method, user);
            SingleResponse<object> response = new SingleResponse<object>();
            try
            {
                user = await _userService.Authenticate(user?.username, user?.password);

                if (user == null)
                {
                    response.ErrorCode = (int)ErrorCode.Unauthorized;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }
                var http_se = HttpContext.Session;

                List<string> user_accesses = SRLCore.Model.UserSession<TUser>.GetAccesses(Db, user.id);

                Dictionary<string, object> Session = new Dictionary<string, object>
                {
                    ["Id"] = user.id,
                    ["Accesses"] = user_accesses,
                    ["UserData"] = new List<TUser> { user }.Select(x => SRLCore.Model.UserSession<TUser>.SessionFields(x)).First()
                };
                response.Model = Session;
                response.ErrorCode = (int)ErrorCode.OK;
            }
            catch (Exception ex)
            {
                LogHandler.LogError(Logger, response, method, ex);
            }

            return response.ToHttpResponse(Logger, Request.HttpContext);
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
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            string method = nameof(AddUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);

            SingleResponse<object> response = new SingleResponse<object>();



            request.pass_mode = PassMode.add;
            request.CheckValidation(response);

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
        public async Task<IActionResult> EditUser([FromBody] AddUserRequest request)
        {
            string method = nameof(EditUser);
            LogHandler.LogMethod(EventType.Call, Logger, method, request);
            SingleResponse<object> response = new SingleResponse<object>();

            try
            {

                request.CheckValidation(response);

                var entity = RequestToEntity(request);
                entity.id = request.id;

                var existingEntity = await Db.GetUser(entity.id, entity.username);
                if (existingEntity == null)
                {
                    response.ErrorCode = (int)ErrorCode.NoContent;
                    return response.ToHttpResponse(Logger, Request.HttpContext);
                }

                existingEntity.first_name = entity.first_name;
                existingEntity.last_name = entity.last_name;
                existingEntity.mobile = entity.mobile;
                //existingEntity.national_code = entity.national_code;
                existingEntity.password = entity.password;

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