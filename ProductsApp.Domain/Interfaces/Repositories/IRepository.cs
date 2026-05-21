using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductsApp.Domain.Models;

namespace ProductsApp.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
    }
}
