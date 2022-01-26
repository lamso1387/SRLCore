using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Net.Http;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using System.Text;
using System.Runtime.CompilerServices;

namespace SRLCore.Model
{
    public class MessageResponse : IResponse
    {

        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }
    }
    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public TModel Model { get; set; }
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public IEnumerable<TModel> Model { get; set; }
    }

    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public IEnumerable<TModel> Model { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int ItemsCount { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 0;
                return ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
            }
        }
    }


}