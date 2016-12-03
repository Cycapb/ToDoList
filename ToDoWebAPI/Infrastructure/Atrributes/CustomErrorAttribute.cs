using System.Text;
using System.Web.Http.Filters;
using NLog;

namespace ToDoWebAPI.Infrastructure.Atrributes
{
    public class CustomErrorAttribute:ExceptionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetLogger("ErrorLogger");
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var message = new StringBuilder();
            message.Append(actionExecutedContext.Exception.Message);
            message.Append("StackTrace: " + actionExecutedContext.Exception.StackTrace);
            Logger.Error(message.ToString);
        }
    }
}