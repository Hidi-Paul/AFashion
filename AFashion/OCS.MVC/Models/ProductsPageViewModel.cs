using System.Collections.Generic;

namespace OCS.MVC.Models
{
    public class ProductsPageViewModel
    {
        public FiltersViewModel FiltersViewModel { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}