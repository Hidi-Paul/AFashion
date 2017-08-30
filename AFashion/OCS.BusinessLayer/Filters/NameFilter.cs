using OCS.DataAccess.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OCS.BusinessLayer.Filters
{
    public class NameFilter : AbstractFilter
    {
        public string Name { get; set; }

        public NameFilter(IEnumerable<Product> source, string name, AbstractFilter otherFilter = null)
            : base(source, otherFilter)
        {
            this.Name = name.ToUpper();
        }

        public override FilterResult Resolve()
        {
            FilterResult results = (Filter != null) ? Filter.Resolve() : new FilterResult();
            var filtered = Source.Where(prod => prod.Name.ToUpper().Contains(Name)).ToList();

            results.AddFilter("Name", filtered);
            return results;
        }
    }
}
