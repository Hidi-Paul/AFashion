using OCS.DataAccess.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OCS.BusinessLayer.Filters
{
    public class CategoryFilter : AbstractFilter
    {
        public string CategName { get; set; }

        public CategoryFilter(IEnumerable<Product> source, string categName, AbstractFilter otherFilter = null)
            : base(source, otherFilter)
        {
            this.CategName = categName;
        }

        public override FilterResult Resolve()
        {
            FilterResult results = (Filter != null) ? Filter.Resolve() : new FilterResult();
            var filtered = Source.Where(prod => prod.Category.Name.Equals(CategName)).ToList();

            results.AddFilter("Category", filtered.ToList());
            return results;
        }
    }
}
