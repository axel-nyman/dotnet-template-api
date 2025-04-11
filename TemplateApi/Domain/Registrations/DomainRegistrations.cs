using TemplateApi.Domain.Abstract;
using TemplateApi.Domain.Services;

namespace TemplateApi.Domain.Registrations
{
    public static class DomainRegistrations
    {
        public static IServiceCollection AddDomainRegistrations(this IServiceCollection services)
        {
            services.AddScoped<IDomainService, DomainService>();
            return services;
        }
    }
}