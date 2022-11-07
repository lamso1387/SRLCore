using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using SRLCore.Model;
using SRLCore.Middleware;

namespace SRLCore.Controllers
{

    public class CommonController<Tcontext, TUser, TRole, TUserRole> : ControllerBase 
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly ILogger Logger;
        protected readonly Tcontext Db;

        protected SRLCore.Services.UserService<Tcontext, TUser, TRole, TUserRole> _userService;
        public long user_session_id =>long.Parse(HttpContext.Session.GetString("Id"));

        public CommonController(IDistributedCache distributedCache, 
            ILogger<CommonController<Tcontext, TUser, TRole, TUserRole>> logger, Tcontext dbContext ,
            SRLCore.Services.UserService<Tcontext, TUser, TRole, TUserRole> userService)  
        {
            _distributedCache = distributedCache;
            Logger = logger;
            Db = dbContext;
            _userService = userService;
            
        }
         
        protected async Task<IActionResult> Add<RequestT,EntityT>(RequestT request) 
            where RequestT : SRLCore.Model.WebRequest where EntityT : CommonProperty
        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);

            var entity = request.ToEntity2<EntityT>();

            await AddSave(Db,entity);

            return response.ToResponse(entity, entity.Selector);
        }

        protected IActionResult Get<EntityT>(EntityT existingEntity,Func<EntityT, object> selector)
              where EntityT : CommonProperty

        {
            SingleResponse<object> response = new SingleResponse<object>();
            existingEntity.ThrowIfNotExist();
            return response.ToResponse(existingEntity, selector);
        }

        protected async Task<IActionResult> Edit<RequestT, EntityT>(RequestT request,EntityT existingEntityAfterEdit)
            where RequestT : WebRequest where EntityT : CommonProperty

        {
            SingleResponse<object> response = new SingleResponse<object>();

            request.CheckValidation(response);
            existingEntityAfterEdit.ThrowIfNotExist(); 

            await UpdateSave(Db);

            return response.ToResponse(existingEntityAfterEdit, existingEntityAfterEdit.Selector);
        }

        protected async Task<IActionResult> Delete<EntityT>(EntityT existingEntity)
            where EntityT : CommonProperty
        {
            SingleResponse<object> response = new SingleResponse<object>();  
            existingEntity.ThrowIfNotExist();

            await RemoveSave(Db,existingEntity);

            return response.ToResponse();
        }

        protected virtual async Task AddSave<T>(Tcontext db, T entity)
        {
            await db.AddAsync(entity);
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        protected virtual async Task UpdateSave(Tcontext db)
        {
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }
        protected virtual async Task RemoveSave<T>(Tcontext db, T entity)
        {
            db.Remove(entity);
            int save = await db.SaveChangesAsync();
            if (save == 0) throw new GlobalException(ErrorCode.DbSaveNotDone);

        }


    }

}