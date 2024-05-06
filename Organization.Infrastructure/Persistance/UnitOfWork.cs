using Organization.Application.Common.Interfaces.Persistance;
using Organization.Infrastructure.Persistance.DataContext;
using Organization.Infrastructure.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly DapperDataContext _dapperDataContext;
        public ICompanyRepository Companies { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public UnitOfWork(DapperDataContext dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
            Init();
        }
        public void Init()
        {
            Companies = new CompanyRepository(_dapperDataContext);
            Employees = new EmployeeRepository(_dapperDataContext);
        }
        public void BeginTransaction()
        {
            _dapperDataContext.Connection.Open();
            _dapperDataContext.Transaction = _dapperDataContext.Connection.BeginTransaction();
        }
        public void Commit()
        {
            _dapperDataContext.Transaction.Commit();
            _dapperDataContext.Transaction.Dispose();
            _dapperDataContext.Transaction = null;
        }
        public void CommitAndCloseConnection()
        {
            _dapperDataContext.Transaction?.Commit();
            _dapperDataContext.Transaction?.Dispose();
            _dapperDataContext.Transaction = null;
            _dapperDataContext.Connection?.Close();
            _dapperDataContext.Connection.Dispose();
        }
        public void RollBack()
        {
            _dapperDataContext.Transaction.Rollback();
            _dapperDataContext.Transaction.Dispose();
            _dapperDataContext.Transaction = null;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dapperDataContext.Transaction?.Dispose();
                    _dapperDataContext.Connection?.Dispose();   
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }
    }
}
