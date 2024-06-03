using Organization.Domain.Common.Models;
using Organization.Domain.Common.Utilities;
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
        Task<IEnumerable<T>> GetAsyncOld(params string[] selectData);
        Task<IEnumerable<T>> GetAsyncNew(QueryParameters queryParameters, params string[] selectData);
        Task<T> GetByIdAsync (string guid, params string[] selectData);
        Task<string> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<int> SoftDeleteAsync(string id, bool deleteFromRelatedChildTables = false);
        Task<int> GetTotalCountAsyc(T entity);
        Task<bool> IsExistingAsync(string disinguishingUniqueKeyValue);
    }
}
