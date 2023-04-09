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
using System.Security.Cryptography;
using System.IO;

namespace SRLCore.Services
{

    public class EncrypService
    {
        public string encryptor { get; set; }

        public EncrypService()
        { 
        }
        public void SetEncryptor(string key)
        {
            encryptor = key;

        }
        public string GetEncryptor()
        {
            return encryptor;

        }

        public string Encrypt(string plainText)
        {
            string key =  GetEncryptor();
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(GetEncryptor());
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        
        

    }

    public class EncrypService2
    {
        public string encryptor { get; set; }
        private readonly IDataProtectionProvider _provider;
        private IDataProtector protector;

        public EncrypService2(IDataProtectionProvider provider)
        {
            _provider = provider;
        }
        public void SetEncryptor(string key)
        {
            encryptor = key;
            protector = _provider.CreateProtector(encryptor);

        }
        public string Encrypt(string plainText)
        {
            return protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        { 
            return protector.Unprotect(cipherText);
        }
    }


}
