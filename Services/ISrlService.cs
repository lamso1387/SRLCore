﻿ 
using SRLCore.Model; 
using System.Net.Http;
using SRLCore.Middleware;
  

namespace SRLCore.Services
{

    public class ISrlService 
    { 
        public readonly IHttpClientFactory _clientFactory;
        public ISrlService(IHttpClientFactory clientFactory)
        { 
            _clientFactory = clientFactory;
        }
        public virtual HttpClient CreateClient(string type)
        {
            var client = _clientFactory.CreateClient(type); 
            return client;
        }

        public void ThrowError(string message=null)
        {
            throw new GlobalException(ErrorCode.FailedDependency, message);
        }


    }
     
}
