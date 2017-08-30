using OCS.DataAccess.DTO;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Filters
{
    public class AbstractFilter
    {
        public IEnumerable<Product> Source;
        public AbstractFilter Filter { get; set; }

        public AbstractFilter()
        {

        }

        public AbstractFilter(IEnumerable<Product> source, AbstractFilter otherFilter = null)
        {
            this.Source = source;
            this.Filter = otherFilter;
        }

        public virtual FilterResult Resolve()
        {
            return new FilterResult();
        }
    }
}
