using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Brand")]
    public class Brand : IEntity
    {
        [Key]
        [Column("BrandID")]
        public Guid ID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Column("BrandName")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class BrandNotFound : Brand
    {

    }
}
