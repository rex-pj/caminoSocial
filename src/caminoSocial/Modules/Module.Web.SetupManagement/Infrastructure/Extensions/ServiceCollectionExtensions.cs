﻿using Microsoft.Extensions.DependencyInjection;

namespace Module.Web.SetupManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
