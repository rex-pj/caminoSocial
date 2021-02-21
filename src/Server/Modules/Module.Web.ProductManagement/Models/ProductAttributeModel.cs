using Camino.Framework.Models;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class ProductAttributeModel : BaseModel
    {
        public ProductAttributeModel()
        {
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
