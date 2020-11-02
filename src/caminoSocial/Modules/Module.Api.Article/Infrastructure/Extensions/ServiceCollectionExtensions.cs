﻿using Module.Api.Article.GraphQL.Resolvers;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using AutoMapper;
using Camino.Service.AutoMap;
using Camino.Framework.Infrastructure.AutoMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Api.Article.GraphQL.Mutations;
using Module.Api.Article.GraphQL.Queries;

namespace Module.Api.Article.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGraphQlServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleCategoryResolver, ArticleCategoryResolver>();
            services.AddTransient<IArticleResolver, ArticleResolver>();

            services.AddGraphQLServer().AddType<ArticleMutations>()
                .AddType<ArticleCategoryMutations>()
                .AddType<ArticleQueries>();
            return services;
        }

        public static IServiceCollection ConfigureContentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutoMapper(typeof(FrameworkMappingProfile), typeof(IdentityMappingProfile));

            services.ConfigureGraphQlServices();
            return services;
        }
    }
}
