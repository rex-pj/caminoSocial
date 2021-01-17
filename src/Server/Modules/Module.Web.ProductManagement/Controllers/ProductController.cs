﻿using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Core.Constants;
using Camino.Core.Enums;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Framework.Helpers.Contracts;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Product;
using System.Linq;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductController : BaseAuthController
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;

        public ProductController(IMapper mapper, IProductBusiness productBusiness, IHttpHelper httpHelper,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _mapper = mapper;
            _productBusiness = productBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProduct)]
        [LoadResultAuthorizations("Product", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ProductFilterModel filter)
        {
            var filterRequest = new ProductFilter()
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

            var productPageList = await _productBusiness.GetAsync(filterRequest);
            var products = productPageList.Collections.Select(x => new ProductModel()
            {
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdateById = x.UpdatedById,
                UpdatedDate = x.UpdatedDate,
                Id = x.Id,
                Name = x.Name,
                ThumbnailId = x.Pictures.Any() ? x.Pictures.FirstOrDefault().Id : 0
            });

            var productPage = new PageListModel<ProductModel>(products)
            {
                Filter = filter,
                TotalPage = productPageList.TotalPage,
                TotalResult = productPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ProductTable", productPage);
            }

            return View(productPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProduct)]
        [LoadResultAuthorizations("Product", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var product = await _productBusiness.FindDetailAsync(id);
                if (product == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductModel()
                {
                    Id = product.Id,
                    UpdateById = product.UpdatedById,
                    UpdatedBy = product.UpdatedBy,
                    CreatedBy = product.CreatedBy,
                    CreatedById = product.CreatedById,
                    CreatedDate = product.CreatedDate,
                    UpdatedDate = product.UpdatedDate,
                    Description = product.Description,
                    Name = product.Name
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProduct)]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProduct)]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var product = _mapper.Map<ProductProjection>(model);
            var exist = _productBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            product.UpdatedById = LoggedUserId;
            product.CreatedById = LoggedUserId;
            var id = await _productBusiness.CreateAsync(product);

            return RedirectToAction("Detail", new { id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProduct)]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productBusiness.FindDetailAsync(id);
            var model = new ProductModel()
            {
                Id = product.Id,
                UpdateById = product.UpdatedById,
                UpdatedBy = product.UpdatedBy,
                CreatedBy = product.CreatedBy,
                CreatedById = product.CreatedById,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                Description = product.Description,
                Name = product.Name
            };

            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProduct)]
        public async Task<IActionResult> Update(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToErrorPage();
            }

            var product = _mapper.Map<ProductProjection>(model);
            if (product.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _productBusiness.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            product.UpdatedById = LoggedUserId;
            await _productBusiness.UpdateAsync(product);
            return RedirectToAction("Detail", new { id = product.Id });
        }
    }
}