using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductAttributeRelationValueMap : EntityMapBuilder<ProductAttributeRelationValue>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttributeRelationValue>()
                .HasTableName(nameof(ProductAttributeRelationValue))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttributeRelation,
                    (relationValue, relation) => relationValue.ProductAttributeRelationId == relation.Id);
        }
    }
}