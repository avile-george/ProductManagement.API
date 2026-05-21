using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.Interfaces.SearchEngines;
using ProductsApp.Domain.Models;

namespace ProductsApp.Application.SearchEngines
{
    public class ProductSearchEngine : GenericNameSearchEngine<Product>, IProductSearchEngine
    {
    }
}
