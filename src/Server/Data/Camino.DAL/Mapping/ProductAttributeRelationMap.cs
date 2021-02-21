using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductAttributeRelationMap : EntityMapBuilder<ProductAttributeRelation>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttributeRelation>()
                .HasTableName(nameof(ProductAttributeRelation))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttribute,
                    (relation, attribute) => relation.ProductAttributeId == attribute.Id)
                .Association(x => x.ProductAttributeRelationValues,
                    (relation, relationValue) => relation.Id == relationValue.ProductAttributeRelationId);
        }
    }
}
