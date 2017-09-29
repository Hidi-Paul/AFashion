using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Product")]
    public class Product : IEntity
    {
        [Key]
        [Column("ProductID")]
        public Guid ID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Column("ProductName")]
        public string Name { get; set; }

        [Required]
        [Range(0, 9999)]
        [Column("ProductPrice")]
        public double Price { get; set; }

        [Required]
        public Brand Brand { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [Column("ProductImageUrl")]
        public string Image { get; set; }
    }

    public class ProductNotFound : Product
    {

    }
}
