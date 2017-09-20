using System.ComponentModel.DataAnnotations;

namespace OCS.BusinessLayer.Models
{
    public class ProductOrderModel
    {
        [Required(AllowEmptyStrings =false)]
        [StringLength(50)]
        public string ProductName { get; set; }
        
        [Required]
        [Range(1,9999)]
        public int ProductQuantity { get; set; }
    }
}
