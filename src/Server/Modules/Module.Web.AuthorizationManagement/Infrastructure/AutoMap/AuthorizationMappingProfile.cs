﻿using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.IdentityManager.Models;
using Module.Web.AuthorizationManagement.Models;
using Camino.Service.Projections.PageList;

namespace Module.Web.AuthorizationManagement.Infrastructure.AutoMap
{
    public class AuthorizationMappingProfile : Profile
    {
        public AuthorizationMappingProfile()
        {
            CreateMap<RoleProjection, RoleModel>();
            CreateMap<RoleModel, RoleProjection>();
            CreateMap<AuthorizationPolicyProjection, AuthorizationPolicyModel>();
            CreateMap<AuthorizationPolicyModel, AuthorizationPolicyProjection>();
            CreateMap<AuthorizationPolicyUsersPageList, AuthorizationPolicyUsersModel>();
            CreateMap<AuthorizationPolicyRolesPageList, AuthorizationPolicyRolesModel>();

            CreateMap<UserRoleAuthorizationPoliciesProjection, ApplicationUserRoleAuthorizationPolicy>();
            CreateMap<RoleAuthorizationPoliciesProjection, ApplicationRole>();
            CreateMap<AuthorizationPolicyProjection, ApplicationAuthorizationPolicy>();
            CreateMap<ApplicationRole, RoleModel>();
            CreateMap<UserProjection, UserModel>();
            CreateMap<UserModel, UserProjection>();

            CreateMap<AuthorizationPolicyFilterModel, AuthorizationPolicyFilter>();
            CreateMap<RoleAuthorizationPolicyFilterModel, RoleAuthorizationPolicyFilter>();
            CreateMap<RoleFilterModel, RoleFilter>();

            CreateMap<UserAuthorizationPolicyFilterModel, UserAuthorizationPolicyFilter>();
        }
    }
}
