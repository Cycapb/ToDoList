using System.Web.Http;
using ToDoWebAPI.Infrastructure;

namespace ToDoWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new CustomErrorAttribute());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("",
                "api/{controller}/{userId}",null,
                new { userId = @"^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
