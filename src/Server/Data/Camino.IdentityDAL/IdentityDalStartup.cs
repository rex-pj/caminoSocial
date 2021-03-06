﻿using Camino.IdentityDAL.Contracts;
using Camino.IdentityDAL.Implementations;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.IdentityDAL
{
    public static class IdentityDalStartup
    {
        public static void AddIdentityDataAccessServices(this IServiceCollection services, string connectionName)
        {
            var configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            services.AddLinqToDbContext<IdentityDbConnection>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionName))
                .UseDefaultLogging(provider);
            });

            services.AddTransient<IIdentityDataProvider, IdentityDataProvider>();
        }
    }
}
