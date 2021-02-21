using System.Collections.Generic;

namespace Camino.DAL.Entities
{
    public class ProductAttributeRelation
    {
		public int Id { get; set; }
		public int ProductAttributeId { get; set; }
		public long ProductId { get; set; }
		public string TextPrompt { get; set; }
		public bool IsRequired { get; set; }
		public int AttributeControlTypeId { get; set; }
		public int DisplayOrder { get; set; }
		public virtual ProductAttribute ProductAttribute { get; set; }
		public virtual ICollection<ProductAttributeRelationValue> ProductAttributeRelationValues { get; set; }
	}
}
