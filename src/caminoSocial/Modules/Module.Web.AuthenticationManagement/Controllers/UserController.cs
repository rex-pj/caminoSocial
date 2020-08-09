﻿using AutoMapper;
using Camino.Business.Contracts;
using Camino.Framework.Controllers;
using Camino.Framework.Models;
using Module.Web.AuthenticationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Camino.Framework.Attributes;
using Camino.Core.Constants;
using Camino.Core.Enums;

namespace Module.Web.AuthenticationManagement.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        [LoadResultAuthorizations("User", PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public IActionResult Index()
        {
            var users = _userBusiness.GetFull();
            var userModels = _mapper.Map<List<UserViewModel>>(users);
            var userPage = new PageListViewModel<UserViewModel>(userModels);

            return View(userPage);
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUser)]
        public IActionResult Search(string q, List<long> currentUserIds)
        {
            var users = _userBusiness.Search(q, currentUserIds);
            if (users == null || !users.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemViewModel>()
                });
            }

            var userModels = _mapper.Map<List<UserViewModel>>(users)
                .Select(x => new Select2ItemViewModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Lastname + " " + x.Firstname
                });

            return Json(userModels);
        }
    }
}