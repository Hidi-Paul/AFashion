using System.Collections.Generic;

namespace OCS.MVC.Models
{
    public class FiltersViewModel
    {
        public string SearchString { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<BrandViewModel> Brands { get; set; }
    }
}