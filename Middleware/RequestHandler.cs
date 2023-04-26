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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Web;
using System.ComponentModel;
using Task = System.Threading.Tasks.Task;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Globalization;
using SRLCore.Model;
using SRLCore.Services;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using SRLCore.Model.Constants;

namespace SRLCore.Middleware
{
    public abstract class HandlerMiddleware<Tcontext, TUser, TRole, TUserRole>
        where TUser : IUser where TRole : IRole where TUserRole : IUserRole
        where Tcontext : DbEntity<Tcontext, TUser, TRole, TUserRole>
    {
        public virtual string[] no_auth_actions => new string[] { "" };
        public virtual string[] no_access_actions => new string[] { "" };
        public virtual bool check_user_first_login => false;
        public abstract DotNetCoreVersion dotnet_core_version { get; }

        protected readonly RequestDelegate _next;

        public HandlerMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public async Task Invoke(HttpContext context, UserService<Tcontext, TUser, TRole, TUserRole> _userService, ILogger Logger)
        {
            Stream response_body = context.Response.Body;
            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;

                string action = null;
                try
                {
                    bool need_auth = context.NeedAuth(no_auth_actions, ref action);
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(
                        context.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: 1000000,
                        leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        LogHandler.LogMethod(EventType.Call, Logger, action, Newtonsoft.Json.JsonConvert.DeserializeObject(body));
                        context.Request.Body.Position = 0;
                    }

                    if (need_auth)
                    {
                        if (!context.Request.Headers.ContainsKey("Authorization"))
                            throw new GlobalException(ErrorCode.Unauthorized);
                        TUser user = null;
                        try
                        {
                            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                            var username = credentials[0];
                            var password = credentials[1];
                            user = await _userService.Authenticate(username, password);
                        }
                        catch
                        {
                            throw new GlobalException(ErrorCode.Unauthorized);
                        }
                        if (user == null) throw new GlobalException(ErrorCode.Unauthorized);

                        if (check_user_first_login && (user.change_pass_next_login == null ? false : (bool)user.change_pass_next_login))
                            throw new GlobalException(ErrorCode.PreconditionFailed, MessageText.PasswordMustBeChanged);

                        context.Session.SetString("Id", user.id.ToString());
                        context.Session.SetString("UserData", Newtonsoft.Json.JsonConvert.SerializeObject(user));

                        bool need_access = context.NeedAccess(no_access_actions, action);
                        bool has_authority = false;
                        List<string> user_accesses = new List<string>();
                        if (action != "authenticate") has_authority = _userService.Authorization(action, user.id, out user_accesses);

                        if (has_authority == false && need_access) throw new GlobalException(ErrorCode.Forbidden);

                        context.Session.SetString("Accesses", Newtonsoft.Json.JsonConvert.SerializeObject(user_accesses));

                        var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.full_name)};
                        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
                        var principal = new ClaimsPrincipal(identity);
                        context.User = principal;
                    }

                    await _next.Invoke(context);

                }

                catch (Exception error)
                {
                    await HandleExceptionAsync(context, error);
                }

                try
                {
                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false).ReadToEnd();
                    memStream.Position = 0;
                    await memStream.CopyToAsync(response_body);
                    context.Response.Body = response_body;
                }
                catch (Exception error)
                {
                    await HandleExceptionAsync(context, error);
                }

            }


        }

        public byte[] ReadAllBytes(Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        protected Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            string output = string.Empty;
            if (!context.Response.HasStarted)
            {
                MessageResponse mes_res = new MessageResponse();
                mes_res.ErrorMessage = error.Message;
                mes_res.ErrorDetail = error.InnerException?.Message;
                mes_res.ErrorCode = (int)ErrorCode.UnexpectedError;
                context.Response.StatusCode = (int)ErrorCode.UnexpectedError;
                ErrorCode error_code=ErrorCode.UnexpectedError;
                ErrorProp error_prop=new ErrorProp();
                switch (error.GetType().Name)
                {
                    case nameof(GlobalException):
                        error_code = EnumConvert.StringToEnum<ErrorCode>(error.Message);
                        error_prop = ErrorProp.GetError(error_code);
                        mes_res.ErrorMessage = error_prop.message;
                        if (!string.IsNullOrWhiteSpace(mes_res.ErrorDetail)) mes_res.ErrorMessage = mes_res.ErrorDetail;
                        break;
                    case nameof(InvalidOperationException):
                        error_code = ErrorCode.InvalidOperationException;
                        error_prop = ErrorProp.GetError(error_code);
                        mes_res.ErrorDetail = $"{mes_res.ErrorMessage}. {mes_res.ErrorDetail}";
                        mes_res.ErrorMessage = error_prop.message;
                        break;
                    case nameof(DbUpdateException):
                        error_code = ErrorCode.DbUpdateException;
                        error_prop = ErrorProp.GetError(error_code);
                        mes_res.ErrorMessage = error_prop.message;
                        break;
                }

                
                context.Response.StatusCode = (int)error_prop.status;               
                mes_res.ErrorCode = (int)error_code; 
                context.Response.ContentType = "application/json";
                output = JsonSerializer.Serialize(mes_res);
            }
            return context.Response.WriteAsync(output);
        }

    }
    public class GlobalException : Exception
    {
        public GlobalException(ErrorCode error_code) : base(error_code.ToString())
        {
        }
        public GlobalException(ErrorCode error_code, string message) :
            base(error_code.ToString(), new Exception(message))
        {
        }
    }

    public class ErrorProp
    {
        public string message { get; set; } = MessageText.ErrorNotSet;
        //  public int code { get; set; } = -1;
        public HttpStatusCode status { get; set; } = HttpStatusCode.Unused;

        public static ErrorProp GetError(ErrorCode key, string message = null)
        {
            string enum_des_str = SRL.ClassManagement.GetEnumDescription(key);
            ErrorProp enum_des = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorProp>(enum_des_str);
            if (message != null) enum_des.message = message;
            return enum_des;
        }
    }
}
