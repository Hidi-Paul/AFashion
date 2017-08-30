using OCS.DataAccess.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OCS.BusinessLayer.Filters
{
    public class FilterResult
    {
        public Dictionary<string, IEnumerable<Product>> Filters;

        public FilterResult()
        {
            Filters = new Dictionary<string, IEnumerable<Product>>();
        }

        public void AddFilter(string key, IEnumerable<Product> values)
        {
            if (Filters.ContainsKey(key))
            {
                Filters[key] = Filters[key].Union(values);
            }
            else
            {
                Filters.Add(key, values);
            }
        }

        public IEnumerable<Product> Result()
        {
            if (Filters == null || Filters.Count == 0)
            {
                return new List<Product>();
            }

            IEnumerable<Product> final = Filters.ElementAt(0).Value;
            for (int i = 1; i < Filters.Count; i++)
            {
                final = final.Intersect(Filters.ElementAt(i).Value);
            }

            return final;
        }
    }
}
