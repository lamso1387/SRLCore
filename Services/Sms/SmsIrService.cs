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
using System.Net.Http;

using System.IO;
using SRLCore.Middleware;
using SmsIrRestful;

namespace SRLCore.Services
{

    public abstract class SmsIrService
    {
        private readonly IHttpClientFactory _clientFactory;

        public SmsIrService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public bool Send(UltraFastSend ultraFastSend)
        {
            var token = GetToken();


            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }

            return ultraFastSendRespone.IsSuccessful;
        }


        public string GetToken()
        {

            Token tk = new Token();

           // string token = tk.GetToken(Startup.setting.SmsIrKey.SecretKey, Startup.setting.SmsIrKey.UserApiKey);


            return null;
        }


    }


}
