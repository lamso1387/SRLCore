using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;
using SRLCore.Middleware;

namespace SRLCore.Model
{
#pragma warning disable CS1591

    
    public static class HttpContextExtentions
    {
        public static string GetActionName(this HttpContext context)
        { 
            var action= context.GetRouteData()?.Values["action"]?.ToString();
            if (action == null) throw new GlobalException(ErrorCode.NotFound);
            return action;
        }
        public static bool NeedAuth(this HttpContext context,string[] no_auth_actions, ref string action)
        {
            action = context.GetActionName();
            return !no_auth_actions.Contains(action);
        } 
        public static bool NeedAccess(this HttpContext context, string[] no_access_actions, string action)
        { 
            return !no_access_actions.Contains(action);
        }
    }
    


#pragma warning restore CS1591
}