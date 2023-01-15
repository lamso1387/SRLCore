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
using Microsoft.AspNetCore.DataProtection;

namespace SRLCore.Services
{

    public class EncrypService
    {
        public string encryptor { get; set; }
        public string decryptor { get; set; }

        private readonly IDataProtectionProvider _provider;

        public EncrypService(IDataProtectionProvider provider)
        {
            _provider = provider;
        }
        public void SetEncryptor(string key)
        {
            encryptor = key;
        }
        public void SetDecryptor(string key)
        {
            decryptor = key;
        }
        public string Encrypt(string plainText)
        {
            var protector = _provider.CreateProtector(encryptor);
            return protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _provider.CreateProtector(decryptor);
            return protector.Unprotect(cipherText);
        }
    }


}
