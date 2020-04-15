using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoDAL.Core.Model;
using ToDoWebAPI.Core.Infrastructure.Migrators;

namespace ToDoWebAPI.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContextCore>(options => 
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(_configuration["ConnectionStrings:TodoEntities:ConnectionString"]);
            });
            services.AddMvc().AddMvcOptions(setupAction => setupAction.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            DatabaseMigrator.MigrateDatabase(app);
        }
    }
}
