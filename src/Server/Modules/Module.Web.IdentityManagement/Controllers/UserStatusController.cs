﻿using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Identity;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.IdentityManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Users.Contracts;

namespace Module.Web.IdentityManagement.Controllers
{
    public class UserStatusController : BaseController
    {
        private readonly IUserStatusBusiness _userStatusBusiness;
        private readonly IHttpHelper _httpHelper;

        public UserStatusController(IHttpContextAccessor httpContextAccessor, IUserStatusBusiness userStatusBusiness,
            IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _userStatusBusiness = userStatusBusiness;
            _httpHelper = httpHelper;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        [LoadResultAuthorizations("UserStatus", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(UserStatusFilterModel filter)
        {
            var filterRequest = new UserStatusFilter()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };

            var statusPageList = await _userStatusBusiness.GetAsync(filterRequest);
            var statuses = statusPageList.Collections.Select(x => new UserStatusModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            var statusPage = new PageListModel<UserStatusModel>(statuses)
            {
                Filter = filter,
                TotalPage = statusPageList.TotalPage,
                TotalResult = statusPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UserStatusTable", statusPage);
            }

            return View(statusPage);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        public IActionResult Search(string q)
        {
            var statuses = _userStatusBusiness.Search(q);
            if (statuses == null || !statuses.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = statuses
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        [LoadResultAuthorizations("UserStatus", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var status = _userStatusBusiness.Find(id);
                if (status == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new UserStatusModel()
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserStatus)]
        public IActionResult Create()
        {
            var model = new UserStatusModel();
            return View(model);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUserStatus)]
        public IActionResult Update(int id)
        {
            var status = _userStatusBusiness.Find(id);
            var model = new UserStatusModel()
            {
                Id = status.Id,
                Name = status.Name,
                Description = status.Description
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateUserStatus)]
        public IActionResult Create(UserStatusModel model)
        {
            var exist = _userStatusBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var status = new UserStatusProjection()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
            var id = _userStatusBusiness.Create(status);

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateUserStatus)]
        public IActionResult Update(UserStatusModel model)
        {
            if (model.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _userStatusBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            var status = new UserStatusProjection()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
            _userStatusBusiness.Update(status);
            return RedirectToAction("Detail", new { id = status.Id });
        }
    }
}
