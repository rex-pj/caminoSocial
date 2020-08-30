﻿using Camino.Core.Constants;
using Camino.Service.FileStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;

namespace Module.Web.UploadManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureFileServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddSingleton(typeof(BaseFileStore), s =>
            {
                var webHostEnvironment = s.GetRequiredService<IWebHostEnvironment>();
                var appDataPath = Path.Combine(webHostEnvironment.ContentRootPath, AppDataSettings.MediaPath);
                return new LocalMediaFileStore(appDataPath);
            });

            return services;
        }
    }
}
