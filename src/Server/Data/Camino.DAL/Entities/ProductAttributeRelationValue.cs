namespace Camino.DAL.Entities
{
    public class ProductAttributeRelationValue
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int ProductAttributeRelationId { get; set; }
		public decimal PriceAdjustment { get; set; }
		public decimal PricePercentageAdjustment { get; set; }
		public int Quantity { get; set; }
		public int DisplayOrder { get; set; }
		public virtual ProductAttributeRelation ProductAttributeRelation { get; set; }
	}
}
