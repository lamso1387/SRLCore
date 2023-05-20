using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRLCore.Model
{
    public class LogHandler
    {

        public static void LogMethod(EventType event_type, ILogger Logger, string method, params object[] parmeters)
        {

            Log(event_type, Logger, method, null, parmeters);
        }
        private static void Log(EventType event_type, ILogger Logger, string method, Exception ex, params object[] parmeters)
        {
            List<string> inputs = new List<string>();
            foreach (object item in parmeters)
            {
                if (item != null)
                    inputs.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented));
                else inputs.Add("null");
            }

            var event_id = new EventId((int)event_type);
            switch (event_type)
            {
                case EventType.Exception:
                    Logger.LogCritical(event_id, ex, $"{event_type.ToString()} {ex.GetType().Name} {method}");
                    break;
                case EventType.Call:
                case EventType.Return:
                case EventType.Operation:
                    Logger.LogInformation(event_id, $"{event_type.ToString()} {method} {string.Join(",", inputs)}");
                    break;

            }
        }
        public static void LogError(ILogger Logger, IResponse response, string method, Exception ex)
        {
            response.ErrorCode = (int)ErrorCode.UnexpectedError;
            response.ErrorDetail = Tools.ExceptionsTools.CreateExactErrorMessage(ex);
            switch (ex.GetType().Name)
            {
                case nameof(DbUpdateException):
                    response.ErrorCode = (int)ErrorCode.DbUpdateException;
                    break;
            }

            Log(EventType.Exception, Logger, method, ex);
        }
        public static void CatchFunction(ILogger Logger, IResponse response, string method, Exception ex)
        {
            switch (ex.GetType().Name)
            {
                case nameof(DbUpdateException):

                    break;
                default:
                    LogError(Logger, response, method, ex);
                    break;
            }
        }
    }
}
