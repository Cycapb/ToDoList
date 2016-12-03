using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace ToDoWebAPI.Infrastructure.Services
{
    public class CustomExceptionLogger:ExceptionLogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var message = new StringBuilder();
            message.AppendLine($"Request URI: {context.Request.RequestUri.AbsolutePath}");
            message.AppendLine("Headers: ");
            foreach (var header in context.Request.Headers)
            {
                message.AppendLine($"{header.Key}: {string.Join(",",header.Value)}");
            }
            message.AppendLine("Cookies:");
            foreach (var cookie in context.Request.Headers.GetCookies())
            {
                foreach (var cookieItem in cookie.Cookies)
                {
                    var values = cookieItem.Values?.Cast<string>().Select(x => cookieItem.Values[x]);
                    var str = string.Join(",",values?? Enumerable.Empty<string>());
                    message.AppendLine($"{cookieItem.Name} : {str}");
                }
            }
            _logger.Log(LogLevel.Error,context.Exception, $"Data: {message}");
            return Task.CompletedTask;
        }
    }
}