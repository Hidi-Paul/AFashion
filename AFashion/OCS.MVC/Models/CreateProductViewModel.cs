using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OCS.MVC.Models
{
    public class CreateProductViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Name required.")]
        [StringLength(50, ErrorMessage = "Product Name is too long!")]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Price is mandatory.")]
        [Range(1, 9999, ErrorMessage = "Invalid Price Range!")]
        public double Price { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Brand is mandatory.")]
        [StringLength(50, ErrorMessage = "Brand too long.")]
        public string Brand { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category is mandatory.")]
        [StringLength(50)]
        public string Category { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add an image.")]
        [StringLength(200)]
        public string Image { get; set; }
    }
}