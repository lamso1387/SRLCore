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
using SRLCore.Model.Response.Kavenagar;
using SRLCore.Model.Enum;

namespace SRLCore.Services
{
    public class KavenegarService :ISrlService
    {
        public string api_key { get; set; }

        public KavenegarService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }


        public async Task<KavenegarListResponse<SendSmsResponse>> Send(List<string> mobiles, string sender, string message)
        {  
            var input = $"?receptor={string.Join(",", mobiles)}&sender={sender}&message={message}";

            var client = CreateClient(ClientType.kavenegar.ToString());

            var response = await client.GetAsync($"{api_key}{Model.Constants.KavenagarUrl.sms_send_json}{input}");

            var result = await response.Content.ReadAsAsync<KavenegarListResponse<SendSmsResponse>>(); 
             
            return result;
        }

    }


}
