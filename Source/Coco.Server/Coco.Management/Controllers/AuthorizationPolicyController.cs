﻿using AutoMapper;
using Coco.Business.Contracts;
using Coco.Common.Helpers;
using Coco.Entities.Dtos.Auth;
using Coco.Framework.Controllers;
using Coco.Management.Common.Enums;
using Coco.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Coco.Management.Controllers
{
    public class AuthorizationPolicyController : BaseAuthController
    {
        private readonly IAuthorizationPolicyBusiness _authorizationPolicyBusiness;
        private readonly IMapper _mapper;
        public AuthorizationPolicyController(IMapper mapper, IAuthorizationPolicyBusiness authorizationPolicyBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        { 
            _mapper = mapper;
            _authorizationPolicyBusiness = authorizationPolicyBusiness;
        }

        public IActionResult Index()
        {
            var policies = _authorizationPolicyBusiness.GetFull();
            var policyModels = _mapper.Map<List<AuthorizationPolicyViewModel>>(policies);
            var policiesPage = new PagerViewModel<AuthorizationPolicyViewModel>(policyModels);

            return View(policiesPage);
        }

        public IActionResult Detail(short id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var policy = _authorizationPolicyBusiness.Find(id);
                if (policy == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<AuthorizationPolicyViewModel>(policy);
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AuthorizationPolicyViewModel()
            {
                SelectPermissionMethods = EnumHelpers.ToSelectListItems<PermissionMethodEnum>()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(short id)
        {
            var policy = _authorizationPolicyBusiness.Find(id);
            var model = _mapper.Map<AuthorizationPolicyViewModel>(policy);

            var permissionMethod = EnumHelpers.FilterEnumByName<PermissionMethodEnum>(model.Name);
            model.SelectPermissionMethods = EnumHelpers.ToSelectListItems(permissionMethod);
            model.PermissionMethod = (int)permissionMethod;
            var permissionMethodName = permissionMethod.ToString();
            model.Name = model.Name.Replace(permissionMethodName, "");

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(AuthorizationPolicyViewModel model)
        {
            if (model.PermissionMethod > 0)
            {
                var permissionMethod = (PermissionMethodEnum)model.PermissionMethod;
                model.Name = $"{permissionMethod}{model.Name}";
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(model);
            policy.UpdatedById = LoggedUserId;
            if(policy.Id > 0)
            {
                _authorizationPolicyBusiness.Update(policy);
                return RedirectToAction("Detail", new { id = policy.Id });
            }

            var exist = _authorizationPolicyBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            policy.CreatedById = LoggedUserId;
            var newId = _authorizationPolicyBusiness.Add(policy);

            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
