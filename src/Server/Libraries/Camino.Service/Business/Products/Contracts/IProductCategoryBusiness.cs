using Camino.Service.Projections.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.DAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductCategoryBusiness
    {
        ProductCategoryProjection Find(long id);
        Task<BasePageList<ProductCategoryProjection>> GetAsync(ProductCategoryFilter filter);
        Task<IList<ProductCategoryProjection>> SearchAsync(long[] currentIds, string search = "", int page = 1, int pageSize = 10);
        Task<IList<ProductCategoryProjection>> SearchParentsAsync(long[] currentIds, string search = "", int page = 1, int pageSize = 10);
        List<ProductCategoryProjection> Get(Expression<Func<ProductCategory, bool>> filter);
        Task<int> CreateAsync(ProductCategoryProjection category);
        Task<ProductCategoryProjection> UpdateAsync(ProductCategoryProjection category);
        ProductCategoryProjection FindByName(string name);
    }
}
