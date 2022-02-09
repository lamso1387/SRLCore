using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Web;
using System.ComponentModel;
using Task = System.Threading.Tasks.Task;
using System.Net;
using Microsoft.AspNetCore.Builder;
using SRLCore.Model;
using Microsoft.AspNetCore.Http;

namespace SRLCore.Services
{

    public class UserService<Tcontext, TUser, TRole, TUserRole>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>

    {
        private readonly Tcontext _context;
        public UserService(Tcontext context)
        {
            _context = context;
        }
        public async Task<TUser> Authenticate(string username, string password)
        {
            TUser user = await _context.Users.FirstOrDefaultAsync(x => x.username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                return null;
            }
            else if (!VerifyPasswordHash(password, user.password_hash, user.password_salt))
            {
                return null;
            }


            return user;


        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            IUser.CreatePasswordHashS(password, out passwordHash, out passwordSalt);

        }



        public bool Authorization(string action, long user_id, out List<string> user_accesses)
        {
            user_accesses = UserSession<TUser>.GetAccesses(_context, user_id);
            bool is_allowed = user_accesses.Distinct().Contains(action);
            return is_allowed;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }

     
}
