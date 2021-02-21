using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Service.Business.Products
{
    public class ProductAttributeBusiness : IProductAttributeBusiness
    {
        private readonly IRepository<ProductAttribute> _productAttributeRepository;

        public ProductAttributeBusiness(IRepository<ProductAttribute> productAttributeRepository)
        {
            _productAttributeRepository = productAttributeRepository;
        }

        public ProductAttributeProjection Find(long id)
        {
            var productAttribute = _productAttributeRepository.Get(x => x.Id == id)
                .Select(x => new ProductAttributeProjection
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                }).FirstOrDefault();

            return productAttribute;
        }

        public ProductAttributeProjection FindByName(string name)
        {
            var productAttribute = _productAttributeRepository.Get(x => x.Name == name)
                .Select(x => new ProductAttributeProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefault();

            return productAttribute;
        }

        public async Task<BasePageList<ProductAttributeProjection>> GetAsync(ProductAttributeFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var productAttributeQuery = _productAttributeRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                productAttributeQuery = productAttributeQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            var query = productAttributeQuery.Select(a => new ProductAttributeProjection
            {
                Description = a.Description,
                Id = a.Id,
                Name = a.Name
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var productAttributes = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ProductAttributeProjection>(productAttributes);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public List<ProductAttributeProjection> Get(Expression<Func<ProductAttribute, bool>> filter)
        {
            var productAttributes = _productAttributeRepository.Get(filter).Select(a => new ProductAttributeProjection
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return productAttributes;
        }

        public async Task<IList<ProductAttributeProjection>> SearchAsync(long[] currentIds, string search = "", int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _productAttributeRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search)
                        || x.Description.ToLower().Contains(search));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var productAttributes = await query
                .Select(x => new ProductAttributeProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return productAttributes;
        }

        public async Task<int> CreateAsync(ProductAttributeProjection productAttribute)
        {
            var newProductAttribute = new ProductAttribute()
            {
                Description = productAttribute.Description,
                Name = productAttribute.Name
            };

            var id = await _productAttributeRepository.AddWithInt32EntityAsync(newProductAttribute);
            return id;
        }

        public async Task<ProductAttributeProjection> UpdateAsync(ProductAttributeProjection category)
        {
            await _productAttributeRepository.Get(x => x.Id == category.Id)
                .Set(x => x.Description, category.Description)
                .Set(x => x.Name, category.Name)
                .UpdateAsync();

            return category;
        }
    }
}