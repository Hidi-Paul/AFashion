using System;
using System.ComponentModel.DataAnnotations;

namespace OCS.MVC.Models
{
    public class OrderViewModel
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage ="Quantity can't be less than 1.")]
        public int ProductQuantity { get; set; }

        public string Image { get; set; }
    }
}