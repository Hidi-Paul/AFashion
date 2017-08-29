using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public Guid BrandID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string BrandName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
