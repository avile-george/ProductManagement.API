using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.Models;

namespace ProductsApp.Domain.Interfaces.SearchEngines
{
    public interface IProductSearchEngine : IGenericNameDescriptionSearchEngine<Product>
    {
    }
}
