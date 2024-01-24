using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance.DataContext
{
    public sealed class DapperDataContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public DapperDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("sqlConnection");
        }
        public IDbConnection Connection
        {
            get
            {
                if (_connection is null || _connection.State != ConnectionState.Open)
                    _connection = new SqlConnection(_connectionString);
                return _connection;
            }
        }
        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }
    }
}
