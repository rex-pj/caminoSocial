using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class ProductAttributeMap : EntityMapBuilder<ProductAttribute>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<ProductAttribute>()
                .HasTableName(nameof(ProductAttribute))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.ProductAttributeRelations,
                    (attribute, relation) => attribute.Id == relation.ProductAttributeId);
        }
    }
}
