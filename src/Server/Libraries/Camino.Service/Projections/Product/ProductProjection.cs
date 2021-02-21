using Camino.Service.Projections.Media;
using System;
using System.Collections.Generic;

namespace Camino.Service.Projections.Product
{
    public class ProductProjection
    {
        public ProductProjection()
        {
            Pictures = new List<PictureRequestProjection>();
            Categories = new List<ProductCategoryProjection>();
            Farms = new List<ProductFarmProjection>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryProjection> Categories { get; set; }
        public IEnumerable<ProductFarmProjection> Farms { get; set; }
        public IEnumerable<PictureRequestProjection> Pictures { get; set; }
    }
}
