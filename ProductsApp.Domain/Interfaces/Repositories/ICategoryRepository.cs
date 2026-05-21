using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Domain.Models;

namespace ProductsApp.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}
