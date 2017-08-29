using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
         public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
