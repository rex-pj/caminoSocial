using Camino.Framework.Models;
using System;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Thumbnails = new List<PictureRequestModel>();
            Farms = new List<ProductFarmModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByIdentityId { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductCategoryRelationModel> Categories { get; set; }
        public IEnumerable<ProductFarmModel> Farms { get; set; }
        public IEnumerable<PictureRequestModel> Thumbnails { get; set; }
    }
}
