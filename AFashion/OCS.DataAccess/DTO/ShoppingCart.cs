using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    public class ShoppingCart
    {
        [Required]
        [Index(IsUnique =true)]
        [Column("ShoppingCartID")]
        public Guid ID { get; set; }

        [Key]
        [Required]
        [Column("ShoppingCartUserName")]
        public string UserName { get; set; }
        
        public virtual IEnumerable<ProductOrder> ProductOrders { get; set; }
    }
}
