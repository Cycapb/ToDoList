using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using ToDoWebAPI.Infrastructure.Atrributes;
using ToDoWebAPI.Infrastructure.Services;

namespace ToDoWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new CustomErrorAttribute());
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
