using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SRLCore.Model.Web
{
    public static partial class HttpClientExtention
    {
        public static void AddToHeader(this HttpClient client, string key, string value)
        {
            client.DefaultRequestHeaders.Add(key, value);
        }
    }
}
