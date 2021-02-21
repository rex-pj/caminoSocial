using Microsoft.AspNetCore.Mvc;
using Module.Web.Navigation.Models;
using System.Collections.Generic;
using System.Linq;

namespace Module.Web.Navigation.ViewComponents
{
    public class MainNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var controllerName = RouteData.Values["controller"].ToString().ToLower();
            var mainNavigation = new MainNavigationModel()
            {
                Name = "Camino",
                SubName = "Community",
                Logo = "/img/logo.png",
                Url = "/",
                TabNavigations = new List<TabNavigationModel>() {
                    new TabNavigationModel()
                    {
                        Name = "Content",
                        Code = "content",
                        Navigations = new List<NavigationModel>()
                        {
                            new NavigationModel()
                            {
                                Icon = "fa fa-home",
                                Name = "Home",
                                Code = "Home",
                                Url = "/Home"
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-newspaper",
                                Name = "Article",
                                Code = "articleNav",
                                Url = "/Article",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Articles",
                                        Code = "article",
                                        Url = "/Article"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Article Categories",
                                        Code = "ArticleCategory",
                                        Url = "/ArticleCategory"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Article Pictures",
                                        Code = "ArticlePicture",
                                        Url = "/ArticlePicture"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fa fa-warehouse",
                                Name = "Farm",
                                Code = "farmNav",
                                Url = "/Farm",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Farms",
                                        Code = "Farm",
                                        Url = "/Farm"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Types",
                                        Code = "FarmType",
                                        Url = "/FarmType"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Pictures",
                                        Code = "FarmPicture",
                                        Url = "/FarmPicture"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-flag",
                                Name = "Community",
                                Code = "CommunityNav",
                                Url = "/Community",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Farm Communities",
                                        Code = "Community",
                                        Url = "/Community"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Community Types",
                                        Code = "CommunityType",
                                        Url = "/CommunityType"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fa fa-apple-alt",
                                Name = "Product",
                                Code = "productNav",
                                Url = "/Product",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Products",
                                        Code = "product",
                                        Url = "/Product"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Categories",
                                        Code = "productCategory",
                                        Url = "/ProductCategory"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Attributes",
                                        Code = "productAttribute",
                                        Url = "/ProductAttribute"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Pictures",
                                        Code = "productPicture",
                                        Url = "/ProductPicture"
                                    }
                                }
                            }
                        }
                    },
                    new TabNavigationModel()
                    {
                        Name = "Security",
                        Code = "security",
                        Navigations = new List<NavigationModel>()
                        {
                            new NavigationModel()
                            {
                                Icon = "far fa-user",
                                Name = "User",
                                Code = "userNav",
                                Url = "/User",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Users",
                                        Code = "User",
                                        Url = "/User"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-user-circle",
                                Name = "Authorization",
                                Code = "authorizationNav",
                                Url = "/AuthorizationPolicy",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Authorization Policies",
                                        Code = "AuthorizationPolicy",
                                        SubRoutes = new List<string>()
                                        {
                                            "UserAuthorizationPolicy",
                                            "RoleAuthorizationPolicy"
                                        },
                                        Url = "/AuthorizationPolicy"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Roles",
                                        Code = "Role",
                                        Url = "/Role"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fas fa-fingerprint",
                                Code = "identityNav",
                                Name = "Identity",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Countries",
                                        Code = "Country",
                                        Url = "/Country"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "UserStatus",
                                        Code = "UserStatus",
                                        Url = "/UserStatus"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            foreach (var tab in mainNavigation.TabNavigations)
            {
                foreach (var navigation in tab.Navigations)
                {
                    foreach (var subNavigation in navigation.SubNavigations)
                    {
                        subNavigation.IsActived = subNavigation.Code.ToLower().Equals(controllerName);
                        if (!subNavigation.IsActived)
                        {
                            subNavigation.IsActived = subNavigation.SubRoutes.Any(x => x.ToLower() == controllerName);
                        }
                    }

                    navigation.IsActived = controllerName == navigation.Code.ToLower()
                        || navigation.SubNavigations.Any(x => x.IsActived);
                }

                tab.IsActived = tab.Navigations.Any(x => x.IsActived);
            }

            return View(mainNavigation);
        }
    }
}
