﻿using AutoMapper;
using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ArticleManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Service.Business.Articles.Contracts;

namespace Module.Web.ArticleManagement.Controllers
{
    public class ArticleController : BaseAuthController
    {
        private readonly IArticleBusiness _articleBusiness;
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public ArticleController(IMapper mapper, IArticleBusiness articleBusiness, IHttpHelper httpHelper,
            IArticleCategoryBusiness articleCategoryBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _mapper = mapper;
            _articleBusiness = articleBusiness;
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("Article", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ArticleFilterModel filter)
        {
            var filterRequest = new ArticleFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById,
                CategoryId = filter.CategoryId
            };

            var articlePageList = await _articleBusiness.GetAsync(filterRequest);
            var articles = _mapper.Map<List<ArticleModel>>(articlePageList.Collections);
            var articlePage = new PageListModel<ArticleModel>(articles)
            {
                Filter = filter,
                TotalPage = articlePageList.TotalPage,
                TotalResult = articlePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ArticleTable", articlePage);
            }

            return View(articlePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadArticle)]
        [LoadResultAuthorizations("ArticleCategory", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var article = _articleBusiness.FindDetail(id);
                if (article == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = _mapper.Map<ArticleModel>(article);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ArticleModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateArticle)]
        public IActionResult Create(ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var article = _mapper.Map<ArticleProjection>(model);
            var exist = _articleBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            article.UpdatedById = LoggedUserId;
            article.CreatedById = LoggedUserId;
            var id = _articleBusiness.Add(article);

            return RedirectToAction("Detail", new { id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public IActionResult Update(int id)
        {
            var article = _articleBusiness.FindDetail(id);
            var model = _mapper.Map<ArticleModel>(article);

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateArticle)]
        public IActionResult Update(ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var article = _mapper.Map<ArticleProjection>(model);
            if (article.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _articleBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            article.UpdatedById = LoggedUserId;
            _articleBusiness.Update(article);
            return RedirectToAction("Detail", new { id = article.Id });
        }
    }
}