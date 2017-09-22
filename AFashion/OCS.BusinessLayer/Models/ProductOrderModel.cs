using System;
using System.ComponentModel.DataAnnotations;

namespace OCS.BusinessLayer.Models
{
    public class ProductOrderModel
    {
        public Guid ProductID { get; set; }
        
        [StringLength(50)]
        public string ProductName { get; set; }


        [Required]
        [Range(0, 9999)]
        public int ProductQuantity { get; set; }

        public string Image { get; set; }
    }
}
