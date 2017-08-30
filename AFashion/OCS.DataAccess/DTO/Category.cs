using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.DataAccess.DTO
{
    [Table("Category")]
    public class Category : IEntity
    {
        [Key]
        [Column("CategoryID")]
        public Guid ID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        [Column("CategoryName")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
