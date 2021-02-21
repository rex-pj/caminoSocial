using Camino.Data.Contracts;
using Camino.Data.MapBuilders;
using Camino.DAL.Contracts;
using Camino.DAL.Mapping;

namespace Camino.DAL.Implementations
{
    public class ContentDataProvider : BaseDataProvider<ContentMappingSchema>, IContentDataProvider
    {
        public ContentDataProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {

        }

        protected override void OnMappingSchemaCreating()
        {
            FluentMapBuilder.ApplyMappingBuilder<UserPhotoMap>()
                .ApplyMappingBuilder<UserPhotoTypeMap>()
                .ApplyMappingBuilder<ArticleMap>()
                .ApplyMappingBuilder<ArticleCategoryMap>()
                .ApplyMappingBuilder<PictureMap>()
                .ApplyMappingBuilder<ArticlePictureMap>()
                .ApplyMappingBuilder<FarmMap>()
                .ApplyMappingBuilder<FarmTypeMap>()
                .ApplyMappingBuilder<FarmPictureMap>()
                .ApplyMappingBuilder<ProductMap>()
                .ApplyMappingBuilder<ProductCategoryMap>()
                .ApplyMappingBuilder<ProductPictureMap>()
                .ApplyMappingBuilder<ProductCategoryRelationMap>()
                .ApplyMappingBuilder<ProductPriceMap>()
                .ApplyMappingBuilder<ProductFarmMap>()
                .ApplyMappingBuilder<ProductAttributeMap>()
                .ApplyMappingBuilder<ProductAttributeRelationMap>()
                .ApplyMappingBuilder<ProductAttributeRelationValueMap>();
        }
    }
}
