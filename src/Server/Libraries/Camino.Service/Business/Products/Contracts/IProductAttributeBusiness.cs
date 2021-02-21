using Camino.DAL.Entities;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductAttributeBusiness
    {
        Task<int> CreateAsync(ProductAttributeProjection productAttribute);
        ProductAttributeProjection Find(long id);
        ProductAttributeProjection FindByName(string name);
        List<ProductAttributeProjection> Get(Expression<Func<ProductAttribute, bool>> filter);
        Task<BasePageList<ProductAttributeProjection>> GetAsync(ProductAttributeFilter filter);
        Task<IList<ProductAttributeProjection>> SearchAsync(long[] currentIds, string search = "", int page = 1, int pageSize = 10);
        Task<ProductAttributeProjection> UpdateAsync(ProductAttributeProjection category);
    }
}