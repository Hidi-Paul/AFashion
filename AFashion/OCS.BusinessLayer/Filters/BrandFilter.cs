using OCS.DataAccess.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OCS.BusinessLayer.Filters
{
    public class BrandFilter : AbstractFilter
    {
        public string BrandName { get; set; }

        public BrandFilter(IEnumerable<Product> source, string brandName, AbstractFilter otherFilter = null)
            : base(source, otherFilter)
        {
            this.BrandName = brandName;
        }

        public override FilterResult Resolve()
        {
            FilterResult results = (Filter != null) ? Filter.Resolve() : new FilterResult();
            var filtered = Source.Where(prod => prod.Brand.Name.Equals(BrandName)).ToList();

            results.AddFilter("Brand", filtered);
            return results;
        }
    }
}
