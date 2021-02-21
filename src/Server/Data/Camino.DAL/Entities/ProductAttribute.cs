using System.Collections.Generic;

namespace Camino.DAL.Entities
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
    }
}
