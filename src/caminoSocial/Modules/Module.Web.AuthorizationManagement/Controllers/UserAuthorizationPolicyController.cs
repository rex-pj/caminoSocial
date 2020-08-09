﻿using AutoMapper;
using Camino.Business.Contracts;
using Camino.Framework.Controllers;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Business.Dtos.General;
using Camino.Framework.Helpers.Contracts;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class UserAuthorizationPolicyController : BaseAuthController
    {
        private readonly IUserAuthorizationPolicyBusiness _userAuthorizationPolicyBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        
        public UserAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor, IUserAuthorizationPolicyBusiness userAuthorizationPolicyBusiness,
            IMapper mapper, IHttpHelper httpHelper) : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _userAuthorizationPolicyBusiness = userAuthorizationPolicyBusiness;
            _mapper = mapper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserAuthorizationPolicy)]
        [LoadResultAuthorizations("UserAuthorizationPolicy", PolicyMethod.CanCreate, PolicyMethod.CanDelete)]
        public IActionResult Index(UserAuthorizationPolicyFilterViewModel filter)
        {
            var filterDto = _mapper.Map<UserAuthorizationPolicyFilterDto>(filter);
            var authorizationUsers = _userAuthorizationPolicyBusiness.GetAuthoricationPolicyUsers(filter.Id, filterDto);

            var authorizationUsersPage = _mapper.Map<AuthorizationPolicyUsersViewModel>(authorizationUsers);
            authorizationUsersPage.Filter = filter;

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserAuthorizationPolicyTable", authorizationUsersPage);
            }

            return View(authorizationUsersPage);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserAuthorizationPolicy)]
        public IActionResult Grant(UserAuthorizationPolicyViewModel model)
        {
            var isSucceed = _userAuthorizationPolicyBusiness.Add(model.UserId, model.AuthorizationPolicyId, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = model.AuthorizationPolicyId });
            }
            return RedirectToAction("Index", new { id = model.AuthorizationPolicyId });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanDeleteUserAuthorizationPolicy)]
        public IActionResult Ungrant(long userId, short authorizationPolicyId)
        {
            var isSucceed = _userAuthorizationPolicyBusiness.Delete(userId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = authorizationPolicyId });
            }
            return RedirectToAction("Index", new { id = authorizationPolicyId });
        }
    }
}