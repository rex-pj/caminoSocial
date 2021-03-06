﻿using Camino.Framework.Infrastructure.Extensions;
using Camino.Framework.Providers.Contracts;
using Camino.Framework.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Camino.IdentityManager.Contracts.Core;
using Camino.Framework.Models.Settings;
using Camino.Service.Infrastructure.Extensions;

namespace Camino.Management.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices(configuration);
            services.AddAuthentication(IdentitySettings.APP_SESSION_SCHEMA).AddCookie(IdentitySettings.APP_SESSION_SCHEMA);
            services.AddServices();
            services.AddDataProtection();

            services.AddScoped<ISetupProvider, SetupProvider>();
            services.AddSingleton<SetupSettings>();

            return services;
        }
    }
}
