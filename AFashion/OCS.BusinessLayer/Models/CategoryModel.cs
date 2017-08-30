using System.ComponentModel.DataAnnotations;

namespace OCS.BusinessLayer.Models
{
    public class CategoryModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
