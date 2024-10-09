using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        public ICompanyRepository Companies { get; }   
        public IEmployeeRepository Employees { get;  }
        public IUserRepository Users { get; }
        void BeginTransaction();
        void Commit();
        void CommitAndCloseConnection();
        void RollBack();
    }
}
