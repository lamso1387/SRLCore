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
using SRLCore.Middleware;

namespace SRLCore.Services
{

    public class ISrlService 
    { 
        private readonly IHttpClientFactory _clientFactory;
        public ISrlService(IHttpClientFactory clientFactory)
        { 
            _clientFactory = clientFactory;
        }
        public HttpClient CreateClient(string type)
        {
            var client = _clientFactory.CreateClient(type); 
            return client;
        }
        public void ThrowError()
        {
            throw new GlobalException(ErrorCode.FailedDependency);
        }


    }

     
}
