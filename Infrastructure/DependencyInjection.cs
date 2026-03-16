using Domain.Interface;
using Infrastructure.DBconnect;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureDI(this IServiceCollection services)
        {
            services.AddDbContext<AppDBconnect>(options =>
            {
                options.UseNpgsql(
					"Host=localhost;Port=5432;Database=user;Username=postgres;Password=koeJ2449k");
            });
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
