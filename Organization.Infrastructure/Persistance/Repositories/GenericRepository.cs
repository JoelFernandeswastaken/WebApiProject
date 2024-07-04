using Dapper;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Application.Common.Utilities;
using Organization.Domain.Common.Models;
using Organization.Domain.Common.Utilities;
using Organization.Infrastructure.Persistance.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : IDbEntity
    {
        protected readonly DapperDataContext _dapperDataContext;
        public GenericRepository(DapperDataContext dapperDataContext) 
        { 
            _dapperDataContext = dapperDataContext;
        }

        public async Task<string> AddAsync(T entity)
        {
            try
            {
                string? result = string.Empty;
                string tableName = typeof(T).GetDbTableName(); 
                string columnNames = typeof(T).GetDbTableColumnNames(new string[0]);
                string columnValues = typeof(T).GetColumnValuesForInsert(entity);

                var parameters = new DynamicParameters();
                parameters.Add("tableName", tableName, DbType.String, ParameterDirection.Input, size: 50);
                parameters.Add("columnNames", columnNames, DbType.String, ParameterDirection.Input);
                parameters.Add("columnValues", columnValues, DbType.String, ParameterDirection.Input);


                var transaction = _dapperDataContext.Transaction;
                // var temp = _dapperDataContext.Connection.ExecuteAsync("spSoftDeleteRecord", parameters, _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                result = await _dapperDataContext.Connection.ExecuteScalarAsync<string>("spInsertRecord", parameters, transaction: _dapperDataContext.Transaction, commandType: CommandType.StoredProcedure);

                if(!string.IsNullOrEmpty(result)) 
                    return result;
                return "";
                // return result.Result;

            }
            catch(Exception ex)
            { 
                throw ex;
            }
            
        }
        public async Task<IEnumerable<T>> GetAsyncV1(params string[] selectData)
        {
            var parameters = new DynamicParameters();
            IEnumerable<T> result;

            var tableName = typeof(T).GetDbTableName();
            var columnNames = typeof(T).GetDbTableColumnNames(selectData);

            parameters.Add("tableName", tableName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
            if(selectData.Length != 0)
                parameters.Add("columns", columnNames, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            result = await _dapperDataContext.Connection.QueryAsync<T>("spGetRecordsV1", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return result;
        }
        public async Task<IEnumerable<T>> GetAsyncV2(QueryParameters queryParameters, params string[] selectData)
        {
            try
            {
                var parameters = new DynamicParameters();
                IEnumerable<T> result;

                parameters.Add("tableName", typeof(T).GetDbTableName(), System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                parameters.Add("pageNumber", queryParameters.PageNo, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parameters.Add("pageSize", queryParameters.PageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parameters.Add("sortOrder", queryParameters.SortOrder, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                if (selectData.Length != 0)
                {
                    parameters.Add("columnName", typeof(T).GetDbTableColumnNames(selectData), System.Data.DbType.String, System.Data.ParameterDirection.Input);
                }

                 result = await _dapperDataContext.Connection.QueryAsync<T>("spGetRecordsV2", parameters, commandType: System.Data.CommandType.StoredProcedure);
                 return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetByIdAsync(string guid, params string[] selectData)
        {
            try
            {
                var parameters = new DynamicParameters();
                T result;

                parameters.Add("tableName", typeof(T).GetDbTableName(), System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                parameters.Add("id", guid, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 22);
                if (selectData.Length != 0)
                {
                    parameters.Add("columms", typeof(T).GetDbTableColumnNames(selectData), System.Data.DbType.String, System.Data.ParameterDirection.Input);
                }

                result =  await _dapperDataContext.Connection.QuerySingleOrDefaultAsync<T>("spGetRecordsById", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }

        public async Task<int> GetTotalCountAsyc(T entity)
        {
            try
            {
                var parameters = new DynamicParameters();
                string tablename = typeof(T).GetDbTableName();
                parameters.Add("tableName", tablename, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);

                return await _dapperDataContext.Connection.ExecuteScalarAsync<int>("spGetTotalRecordsCount", parameters, commandType: System.Data.CommandType.StoredProcedure);             
            }
            catch(Exception ex) { throw ex; }
            
        }

        public async Task<int> SoftDeleteAsync(string id, bool deleteFromRelatedChildTables)
        {
            try
            {
                //id = "76cf8907-8b76-4ae4-a25";
               // string tableName = "tblCompanies";
                string tableName = typeof(T).GetDbTableName();
                var parameters = new DynamicParameters();
                parameters.Add("tableName", tableName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);        
                parameters.Add("id", id, DbType.String, System.Data.ParameterDirection.Input);
                var rowsAffectedParentTable = _dapperDataContext.Connection.ExecuteAsync("spSoftDeleteRecord", parameters, _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                int result = rowsAffectedParentTable.Result;
                if (deleteFromRelatedChildTables)
                {
                    foreach (var associatedType in typeof(T).GetAssociatedTypes())
                    {
                        string tablename = associatedType.Type.GetDbTableName();
                        string columnName = associatedType.ForeignKeyProperty;
                        parameters = new DynamicParameters();
                        parameters.Add("tableName", tablename, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                        parameters.Add("foreignKeyColumnName", columnName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                        parameters.Add("foreignKeyColumnValue", id, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 22);

                        var rowsAffectedChildTable = _dapperDataContext.Connection.ExecuteAsync("spSoftDeleteForeignKeyRecord", parameters, _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                        result += rowsAffectedChildTable.Result;
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                int rowsAffected = 0;
                string tableName = typeof(T).GetDbTableName();
                string columnValuesForUpdate = typeof(T).GetColumnValuesForUpdate(entity);
                System.Diagnostics.Debug.Write($"colunvaluesforupdate:{columnValuesForUpdate}");
                string id = entity.Id;

                var parameters = new DynamicParameters();
                parameters.Add("tableName", tableName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                parameters.Add("columnToUpdate", columnValuesForUpdate, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parameters.Add("id", id, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    rowsAffected = await _dapperDataContext.Connection.ExecuteAsync("spUpdateRecord", parameters, transaction: _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                if (rowsAffected > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public bool Update(T entity)
        {
            try
            {
                int rowsAffected = 0;
                string tableName = typeof(T).GetDbTableName();
                string columnValuesForUpdate = typeof(T).GetColumnValuesForUpdate(entity);
                System.Diagnostics.Debug.Write($"colunvaluesforupdate:{columnValuesForUpdate}");
                string id = entity.Id;

                var parameters = new DynamicParameters();
                parameters.Add("tableName", tableName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                parameters.Add("columnToUpdate", columnValuesForUpdate, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parameters.Add("id", id, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                rowsAffected = _dapperDataContext.Connection.Execute("spUpdateRecord", parameters, transaction: _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);

                if (rowsAffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> IsExistingAsync(string disinguishingUniqueKeyValue)
        {
            try
            {
                string tableName = typeof(T).GetDbTableName();
                string distinguishingUniqueKeyName = typeof(T).GetDistinguishingUniqueKeyName();
                var parameters = new DynamicParameters();
                bool isExist = false;
                parameters.Add("tableName", tableName, System.Data.DbType.String, System.Data.ParameterDirection.Input, size: 50);
                parameters.Add("distinguishingUniqueKeyColumnName", distinguishingUniqueKeyName, System.Data.DbType.String, size: 100);
                parameters.Add("distinguishingUniqueKeyColumnValue", disinguishingUniqueKeyValue, System.Data.DbType.String, size: 100);
                
                isExist = await _dapperDataContext.Connection.QuerySingleOrDefaultAsync<bool>("spDoesRecordExist", parameters, _dapperDataContext.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                return isExist;
            }
            catch(Exception ex)
            {
                throw ex;
            }                
        }

    }
}
