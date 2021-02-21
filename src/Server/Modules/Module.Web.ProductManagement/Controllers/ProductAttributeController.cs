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
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Product;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductAttributeController : BaseAuthController
    {
        private readonly IProductAttributeBusiness _productAttributeBusiness;
        private readonly IHttpHelper _httpHelper;

        public ProductAttributeController(IProductAttributeBusiness productAttributeBusiness,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _productAttributeBusiness = productAttributeBusiness;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        [LoadResultAuthorizations("ProductAttribute", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ProductAttributeFilterModel filter)
        {
            var filterRequest = new ProductAttributeFilter()
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search
            };

            var productAttributePageList = await _productAttributeBusiness.GetAsync(filterRequest);
            var productAttributes = productAttributePageList.Collections.Select(x => new ProductAttributeModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name
            });

            var productAttributePage = new PageListModel<ProductAttributeModel>(productAttributes)
            {
                Filter = filter,
                TotalPage = productAttributePageList.TotalPage,
                TotalResult = productAttributePageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ProductAttributeTable", productAttributePage);
            }

            return View(productAttributePage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        [LoadResultAuthorizations("ProductAttribute", PolicyMethod.CanUpdate)]
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var productAttribute = _productAttributeBusiness.Find(id);
                if (productAttribute == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductAttributeModel()
                {
                    Id = productAttribute.Id,
                    Description = productAttribute.Description,
                    Name = productAttribute.Name
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductAttribute)]
        public IActionResult Create()
        {
            var model = new ProductAttributeModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductAttribute)]
        public async Task<IActionResult> Create(ProductAttributeModel model)
        {
            var productAttribute = new ProductAttributeProjection()
            {
                Description = model.Description,
                Name = model.Name
            };

            var exist = _productAttributeBusiness.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            var id = await _productAttributeBusiness.CreateAsync(productAttribute);

            return RedirectToAction("Detail", new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public IActionResult Update(int id)
        {
            var exist = _productAttributeBusiness.Find(id);
            var model = new ProductAttributeModel()
            {
                Id = exist.Id,
                Description = exist.Description,
                Name = exist.Name
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductAttribute)]
        public async Task<IActionResult> Update(ProductAttributeModel model)
        {
            var productAttribute = new ProductAttributeProjection()
            {
                Description = model.Description,
                Name = model.Name,
                Id = model.Id
            };

            if (productAttribute.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = _productAttributeBusiness.Find(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            await _productAttributeBusiness.UpdateAsync(productAttribute);
            return RedirectToAction("Detail", new { id = productAttribute.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductAttribute)]
        public async Task<IActionResult> Search(string q, string currentId = null)
        {
            long[] currentIds = null;
            if (!string.IsNullOrEmpty(currentId))
            {
                currentIds = currentId.Split(',').Select(x => long.Parse(x)).ToArray();
            }

            var productAttributes = await _productAttributeBusiness.SearchAsync(currentIds, q);
            if (productAttributes == null || !productAttributes.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var attributeSeletions = productAttributes
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(attributeSeletions);
        }
    }
}