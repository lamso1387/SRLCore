using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SRLCore.Tools
{
    public partial class ExceptionsTools
    {

        public static string GetExeptionFileLine(Exception ex)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame 
            StackFrame frame = st?.GetFrame(0);
            // Get the line number from the stack frame
            string file = frame?.GetFileName();
            int? line = frame?.GetFileLineNumber();

            return file + ": " + line;
        }
        public static string CreateErrorMessage(Exception ex)
        {
            string error = $"Error, Line={Tools.ExceptionsTools.GetExeptionFileLine(ex)},Type={ex.GetType().FullName},Message={ex.Message} \n StackTrace:  {ex.StackTrace}";
            if (ex.InnerException != null)
            {
                error += "\n InnerException " + CreateErrorMessage(ex.InnerException);
            }
            return error;
        }

        public static string CreateDbEntityExceptionMessage(DbEntityValidationException ee)
        {
            string error = "";
            foreach (var eve in ee.EntityValidationErrors)
            {
                error += $"Entity of type '{eve.Entry.Entity.GetType().Name}' in state '{ eve.Entry.State}' has the following validation errors: ";
                foreach (var ve in eve.ValidationErrors)
                {
                    error += $"\n Property: {ve.PropertyName} , Error: {ve.ErrorMessage}";
                }
                error += "\n";
            }
            return error;
        }
        public static string CreateHttpRequestExMes(HttpRequestException requestException)
        {
            string error = $"\n {requestException.GetType().Name}: {requestException.Message}";
            if (requestException.InnerException is WebException && ((WebException)requestException.InnerException).Status == WebExceptionStatus.NameResolutionFailure)
            {
                error += "\n" + $"{requestException.InnerException.GetType().Name}: {requestException.InnerException.Message}";
            }
            else if (requestException.InnerException != null)
            {
                error += "\n InnerException " + CreateErrorMessage(requestException.InnerException);
            }

            return error;
        }
        public static string CreateAggregateExceptionMessage(AggregateException ae)
        {
            string error = "";
            foreach (var ex in ae.InnerExceptions)
                error += "\n" + $"{ex.GetType().Name}: {ex.Message}";

            return error;
        }


        public static string CreateExactErrorMessage(Exception ex)
        {
            var type_ = ex.GetType().Name;
            switch (type_)
            {
                case nameof(AggregateException):
                    return CreateAggregateExceptionMessage((AggregateException)ex);
                case nameof(HttpRequestException):
                    return CreateHttpRequestExMes((HttpRequestException)ex);
                case nameof(DbEntityValidationException):
                    return CreateDbEntityExceptionMessage((DbEntityValidationException)ex);
                default:
                    return CreateErrorMessage(ex);
            }
        }
    }
}
