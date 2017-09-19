using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OCS.BusinessLayer.Models
{
    public class CreateProductModel
    {
        public Guid ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Range(0, 9999)]
        public double Price { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Category { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ImageExtension { get; set; }

        [Required(AllowEmptyStrings = false)]
        public byte[] Image { get; set; }
    }
}
