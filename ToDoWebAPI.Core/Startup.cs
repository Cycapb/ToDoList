using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ToDoBussinessLogic.Providers;
using ToDoDAL.Abstract;
using ToDoDAL.Concrete;
using ToDoDAL.Core.Model;
using ToDoDAL.Model;
using ToDoProviders;
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
            services
                .AddMvc()
                .AddMvcOptions(setupAction =>
                {
                    setupAction.EnableEndpointRouting = false;
                })
                .AddXmlDataContractSerializerFormatters();
            services.AddTransient<IRepository<TodoItem>, EntityRepositoryCore<TodoItem, TodoContextCore>>();
            services.AddTransient<IRepository<TodoGroup>, EntityRepositoryCore<TodoGroup, TodoContextCore>>();
            services.AddTransient<IEntityValueProvider<TodoItem>, TodoItemProvider>();
            services.AddTransient<IEntityValueProvider<TodoGroup>, TodoGroupProvider>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeSerilog();

            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseMvc();

            DatabaseMigrator.MigrateDatabase(app);
        }

        private void InitializeSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }
    }
}
