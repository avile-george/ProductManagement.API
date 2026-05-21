using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Application.Extensions;
using ProductsApp.Domain.Models;

namespace ProductsApp.Application.SearchEngines
{
    public abstract class GenericNameSearchEngine<T> where T : NamedEntity
    {
        public IEnumerable<T> GetByFuzzyMatch(IEnumerable<T> items, string query)
        {
            if (string.IsNullOrWhiteSpace(query) || items?.Any() != true)
            {
                return items ?? [];
            }

            query = query.ToLower().Trim(); 

            var maxDistance = Convert.ToInt32(query.Length * 0.4) ; // Allowing up to 40% mismatch. In a real application, this could be a parameter, config value

            var matches = items.FindAllMatches(query, maxDistance,
                item => item.Name);

            return matches;
        }
    }
}
