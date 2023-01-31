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

    public abstract class CommonEntityRequestController<Tcontext, TUser, TRole, TUserRole, TEntity, TRequest> :
         CommonController<Tcontext, TUser, TRole, TUserRole>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
        where TEntity : ICommonProperty
         where TRequest : WebRequest
    {


        protected virtual void SetAddEditProperty(TEntity entity, RequestType type, long? edit_id)
        {
            if (type==RequestType.add)
            {
                entity.creator_id = user_session_id;
            }
            else if(type == RequestType.edit)
            {
                entity.modifier_id = user_session_id;
                entity.modify_date = DateTime.Now;
                entity.id = (long)edit_id;
            }
        }
        protected abstract TEntity RequestToEntity (TRequest request);

        protected virtual TEntity CreateEntityFromRequest(TRequest request, RequestType type)
        {
            TEntity entity = RequestToEntity(request);
            SetAddEditProperty(entity, type, request.id);
            return entity;
        }
        protected async virtual Task<TEntity> AddEntity(TRequest request, Tcontext db )
        {
            request.CheckValidation();

            var entity = CreateEntityFromRequest(request, RequestType.add);

            await db.AddSave(entity);
            
            return entity;
        }

        public CommonEntityRequestController(IDistributedCache distributedCache,
        ILogger<CommonEntityRequestController<Tcontext, TUser, TRole, TUserRole, TEntity, TRequest>> logger, Tcontext dbContext,
        Services.UserService<Tcontext, TUser, TRole, TUserRole> userService) :
            base(distributedCache, logger, dbContext, userService)
        {
        }

    }
}