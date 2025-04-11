using Microsoft.EntityFrameworkCore;

using TemplateApi.Domain.Abstract;
using TemplateApi.Infrastructure.Data.Context;
using TemplateApi.Infrastructure.Data.Services;

namespace TemplateApi.Infrastructure.Data.Registrations
{
    public static class DataRegistrations
    {
        public static IServiceCollection AddDataRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDataService, DataService>();
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    string connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ArgumentException("Connectionstring is missing");

                    // TODO: Change database provider by replacing UseMySql with UseNpgsql for PostgreSQL or UseSqlServer for SQL Server
                    ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
                    options.UseMySql(connectionString, serverVersion);
                });

            return services;
        }
    }
}