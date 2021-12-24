using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using SRLCore.Middleware;
using System.Net;
using System.Linq;

namespace SRLCore.Model
{
    public static class ResponseExtension
    {
        public static IActionResult ToHttpResponse(this IResponse response, ILogger Logger, HttpContext context)
        {//should be deleted
            var error = ErrorProp.GetError((ErrorCode)response.ErrorCode, response.ErrorMessage);
            response.ErrorMessage = error.message;
            return CreateHttpObject(response, error.status);

        }

        public static IActionResult ToResponse<T>(this IResponse response, T entity, Func<T, object> selector)
        {
            var model = new List<T> { entity }.Select(selector).First();
            return ToResponse(response, model);
        }
        public static IActionResult ToResponse<T>(this IResponse response, List<T> entity_list, Func<T, object> selector)
        {
            var model = entity_list.Select(selector).ToList();
            return ToResponse(response, model);
        }
        public static IActionResult ToResponse<T>(this IResponse response, T model)
        {
            (response as dynamic).Model = model;
            return ToResponse(response);
        }

        public static IActionResult ToResponse(this IResponse response)
        {
            ErrorCode error_code = ErrorCode.OK;
            response.ErrorCode = (int)error_code;
            var error = ErrorProp.GetError(error_code);
            response.ErrorMessage = error.message;
            return CreateHttpObject(response, error.status);
        }

        private static IActionResult CreateHttpObject(object response, HttpStatusCode status)
        {
            ObjectResult result = new ObjectResult(response);
            result.StatusCode = (int)status;
            return result;
        }


    }
}
