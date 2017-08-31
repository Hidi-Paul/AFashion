using OCS.BusinessLayer.Models;
using System.Collections.Generic;

namespace OCS.WebApi.Models
{
    public class FiltersModel
    {
        public string SearchString { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
        public IEnumerable<BrandModel> Brands { get; set; }
    }
}