using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    public class ProductOrder
    {
        [Key]
        [Column("ProductOrderID")]
        public Guid ID { get; set; }

        [Column("ProductOrderShoppingCart")]
        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        [Column("ProductOrderProduct")]
        public Product Product { get; set; }

        [Required]
        [Range(0,9999)]
        [Column("Quantity")]
        public int Quantity { get; set; }
    }
}
