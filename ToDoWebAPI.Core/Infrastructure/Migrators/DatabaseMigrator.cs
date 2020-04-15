using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoDAL.Core.Model;

namespace ToDoWebAPI.Core.Infrastructure.Migrators
{
    public static class DatabaseMigrator
    {
        public static void MigrateDatabase(IApplicationBuilder applicationBuilder)
        {
            var context = applicationBuilder.ApplicationServices.GetService<TodoContextCore>();
            context.Database.Migrate();
        }
    }
}