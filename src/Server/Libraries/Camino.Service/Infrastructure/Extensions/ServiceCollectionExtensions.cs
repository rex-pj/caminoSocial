using Camino.DAL;
using Camino.DAL.Entities;
using Camino.DAL.Implementations;
using Camino.Data.Contracts;
using Camino.IdentityDAL;
using Camino.IdentityDAL.Entities;
using Camino.IdentityDAL.Implementations;
using Camino.Service.Business.Articles;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Business.Authentication;
using Camino.Service.Business.Authentication.Contracts;
using Camino.Service.Business.Authorization;
using Camino.Service.Business.Authorization.Contracts;
using Camino.Service.Business.Farms;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Business.Feeds;
using Camino.Service.Business.Feeds.Contracts;
using Camino.Service.Business.Identities;
using Camino.Service.Business.Identities.Contracts;
using Camino.Service.Business.Media;
using Camino.Service.Business.Media.Contracts;
using Camino.Service.Business.Products;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Business.Setup;
using Camino.Service.Business.Setup.Contracts;
using Camino.Service.Business.Users;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Strategies.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Service.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddContentDataAccessServices("CaminoEntities");
            services.AddIdentityDataAccessServices("IdentityEntities");

            services.AddIdentityRepositories();
            services.AddContentRepositories();
            services.AddBusinesses();
        }

        public static void AddBusinesses(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>()
                .AddTransient<IAuthenticationBusiness, AuthenticationBusiness>()
                .AddTransient<IUserStatusBusiness, UserStatusBusiness>()
                .AddTransient<ICountryBusiness, CountryBusiness>()
                .AddTransient<IUserPhotoBusiness, UserPhotoBusiness>()
                .AddTransient<IRoleBusiness, RoleBusiness>()
                .AddTransient<IUserAttributeBusiness, UserAttributeBusiness>()
                .AddTransient<IArticleCategoryBusiness, ArticleCategoryBusiness>()
                .AddTransient<IArticlePictureBusiness, ArticlePictureBusiness>()
                .AddTransient<IUserRoleBusiness, UserRoleBusiness>()
                .AddTransient<IAuthorizationPolicyBusiness, AuthorizationPolicyBusiness>()
                .AddTransient<IRoleAuthorizationPolicyBusiness, RoleAuthorizationPolicyBusiness>()
                .AddTransient<IUserAuthorizationPolicyBusiness, UserAuthorizationPolicyBusiness>()
                .AddTransient<IUserClaimBusiness, UserClaimBusiness>()
                .AddTransient<IUserTokenBusiness, UserTokenBusiness>()
                .AddTransient<IUserLoginBusiness, UserLoginBusiness>()
                .AddTransient<IRoleClaimBusiness, RoleClaimBusiness>()
                .AddTransient<ISeedDataBusiness, SeedDataBusiness>()
                .AddTransient<IIdentityDataSetupBusiness, IdentityDataSetupBusiness>()
                .AddTransient<IContentDataSetupBusiness, ContentDataSetupBusiness>()
                .AddTransient<IArticleBusiness, ArticleBusiness>()
                .AddTransient<IPictureBusiness, PictureBusiness>()
                .AddTransient<IFarmBusiness, FarmBusiness>()
                .AddTransient<IFarmTypeBusiness, FarmTypeBusiness>()
                .AddTransient<IFarmPictureBusiness, FarmPictureBusiness>()
                .AddTransient<IProductBusiness, ProductBusiness>()
                .AddTransient<IProductCategoryBusiness, ProductCategoryBusiness>()
                .AddTransient<IProductPictureBusiness, ProductPictureBusiness>()
                .AddTransient<IFeedBusiness, FeedBusiness>()
                .AddTransient<IProductAttributeBusiness, ProductAttributeBusiness>()
                .AddTransient<ValidationStrategyContext>();
        }

        public static void AddIdentityRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<User>, IdentityRepository<User>>()
                .AddTransient<IRepository<UserInfo>, IdentityRepository<UserInfo>>()
                .AddTransient<IRepository<Gender>, IdentityRepository<Gender>>()
                .AddTransient<IRepository<Country>, IdentityRepository<Country>>()
                .AddTransient<IRepository<Role>, IdentityRepository<Role>>()
                .AddTransient<IRepository<UserRole>, IdentityRepository<UserRole>>()
                .AddTransient<IRepository<UserAttribute>, IdentityRepository<UserAttribute>>()
                .AddTransient<IRepository<UserRole>, IdentityRepository<UserRole>>()
                .AddTransient<IRepository<AuthorizationPolicy>, IdentityRepository<AuthorizationPolicy>>()
                .AddTransient<IRepository<UserAuthorizationPolicy>, IdentityRepository<UserAuthorizationPolicy>>()
                .AddTransient<IRepository<RoleAuthorizationPolicy>, IdentityRepository<RoleAuthorizationPolicy>>()
                .AddTransient<IRepository<UserClaim>, IdentityRepository<UserClaim>>()
                .AddTransient<IRepository<UserToken>, IdentityRepository<UserToken>>()
                .AddTransient<IRepository<UserLogin>, IdentityRepository<UserLogin>>()
                .AddTransient<IRepository<RoleClaim>, IdentityRepository<RoleClaim>>()
                .AddTransient<IRepository<Status>, IdentityRepository<Status>>();
        }

        public static void AddContentRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Product>, ContentRepository<Product>>()
                .AddTransient<IRepository<ArticleCategory>, ContentRepository<ArticleCategory>>()
                .AddTransient<IRepository<UserPhoto>, ContentRepository<UserPhoto>>()
                .AddTransient<IRepository<Article>, ContentRepository<Article>>()
                .AddTransient<IRepository<Picture>, ContentRepository<Picture>>()
                .AddTransient<IRepository<ArticlePicture>, ContentRepository<ArticlePicture>>()
                .AddTransient<IRepository<Farm>, ContentRepository<Farm>>()
                .AddTransient<IRepository<FarmType>, ContentRepository<FarmType>>()
                .AddTransient<IRepository<FarmPicture>, ContentRepository<FarmPicture>>()
                .AddTransient<IRepository<ProductCategory>, ContentRepository<ProductCategory>>()
                .AddTransient<IRepository<ProductPicture>, ContentRepository<ProductPicture>>()
                .AddTransient<IRepository<ProductCategoryRelation>, ContentRepository<ProductCategoryRelation>>()
                .AddTransient<IRepository<ProductPrice>, ContentRepository<ProductPrice>>()
                .AddTransient<IRepository<FarmProduct>, ContentRepository<FarmProduct>>()
                .AddTransient<IRepository<ProductAttribute>, ContentRepository<ProductAttribute>>();
        }
    }
}
