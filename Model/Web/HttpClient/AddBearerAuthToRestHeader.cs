using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SRLCore.Model.Web
{
    public static partial   class HttpClientExtention
    {
        public static void AddBearerAuthToHeader(this HttpClient client, string token)
        {
            string Bearer = $"Bearer {token}";
            client.DefaultRequestHeaders.Add("Authorization", Bearer);
        }
    }
}
