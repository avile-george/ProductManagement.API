using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.Interfaces.Repositories;
using ProductsApp.Domain.Models;

namespace ProductsApp.Data.Repositories
{
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {
    }
}
