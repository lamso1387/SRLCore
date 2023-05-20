using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SRLCore.Tools
{
    public partial class JsonTools
    {


        public static ByteArrayContent CreateJsonBody(object request)
        {
            var myContent = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }









    }
}
