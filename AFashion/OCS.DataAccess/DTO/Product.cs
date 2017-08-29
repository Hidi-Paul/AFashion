using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        
        [Required]
        [Index(IsUnique =true)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, 9999)]
        public double ProductPrice { get; set; }

        [Required]
        public Brand Brand { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
