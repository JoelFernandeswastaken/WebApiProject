using Organization.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Persistance
{
    public interface IGenericRepository<T>
        where T : IDbEntity
    {
        Task<IEnumerable<T>> GetAsync(params string[] selectData);
        Task<T> GetByIdAsync (string gui, params string[] selectData);
        Task<string> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task SoftDeleteAsync(string id, bool deleteFromRelatedChildTables = false);
        Task<int> GetTotalCountAsyc(T entity);
    }
}
