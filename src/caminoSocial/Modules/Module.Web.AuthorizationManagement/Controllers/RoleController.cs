﻿using AutoMapper;
using Camino.Business.Contracts;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthorizationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Core.Enums;
using Camino.Business.Dtos.General;
using Camino.Framework.Helpers.Contracts;

namespace Module.Web.AuthorizationManagement.Controllers
{
    public class RoleController : BaseAuthController
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly IMapper _mapper;
        private readonly IApplicationRoleManager<ApplicationRole> _roleManager;
        private readonly IHttpHelper _httpHelper;

        public RoleController(IMapper mapper, IRoleBusiness roleBusiness, IHttpContextAccessor httpContextAccessor,
            IApplicationRoleManager<ApplicationRole> roleManager, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _httpHelper = httpHelper;
            _roleBusiness = roleBusiness;
            _roleManager = roleManager;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        [LoadResultAuthorizations("Role", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(RoleFilterViewModel filter)
        {
            var filterDto = _mapper.Map<RoleFilterDto>(filter);
            var rolePageList = await _roleBusiness.GetAsync(filterDto);
            var roleModels = _mapper.Map<List<RoleViewModel>>(rolePageList.Collections);
            var rolePage = new PageListViewModel<RoleViewModel>(roleModels)
            {
                Filter = filter,
                TotalPage = rolePageList.TotalPage,
                TotalResult = rolePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_RoleTable", rolePage);
            }

            return View(rolePage);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        public IActionResult Search(string q, List<long> currentRoleIds)
        {
            var roles = _roleBusiness.Search(q, currentRoleIds);
            if (roles == null || !roles.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemViewModel>()
                });
            }

            var userModels = _mapper.Map<List<RoleViewModel>>(roles)
                .Select(x => new Select2ItemViewModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        [LoadResultAuthorizations("Role", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(byte id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<RoleViewModel>(role);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadRole)]
        public IActionResult Create()
        {
            var model = new RoleViewModel();

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        public IActionResult Create(RoleViewModel model)
        {
            if (model.Id > 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _roleBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var role = _mapper.Map<ApplicationRole>(model);
            role.UpdatedById = LoggedUserId;
            role.CreatedById = LoggedUserId;
            var newId = _roleManager.CreateAsync(role);
            return RedirectToAction("Detail", new { id = newId });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateRole)]
        public async Task<IActionResult> Update(long id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var model = _mapper.Map<RoleViewModel>(role);

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateRole)]
        public IActionResult Update(RoleViewModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _roleBusiness.FindByName(model.Name);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var role = _mapper.Map<ApplicationRole>(model);
            role.UpdatedById = LoggedUserId;
            var newId = _roleManager.UpdateAsync(role);
            return RedirectToAction("Detail", new { id = newId });
        }
    }
}
