using Application;
using Infrastructure;

namespace User.api
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApiDI(this IServiceCollection services)
        {
            services.ApplicationDI()
                    .InfrastructureDI();
            return services;
        }
    }
}
