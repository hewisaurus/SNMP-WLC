using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Connection;

namespace Database
{
    public abstract class RepositoryBase
    {
        protected readonly IConnectionFactory _connectionFactory;
        protected readonly string _connectionString;

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connectionString = connectionFactory.GetConnectionString();
        }

        protected DbConnection OpenConnection()
        {
            var connection = _connectionFactory.Create();
            connection.Open();
            return connection;
        }

        protected async Task<DbConnection> OpenConnectionAsync()
        {
            var connection = _connectionFactory.Create();
            await connection.OpenAsync();
            return connection;
        }

        protected T Query<T>(Func<DbConnection, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            
            using (var connection = OpenConnection())
            {
                var result = func(connection);
                return result;
            }
        }

        protected async Task<T> QueryAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            try
            {
                using (var connection = await OpenConnectionAsync())
                {
                    return await func(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(string.Format("{0}.QueryAsync() timed out", GetType().FullName));
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in QueryAsync()", ex);
            }
            //catch (SqlException ex)
            //{
            //    throw new Exception($"{GetType().FullName}.ExecuteAsync() experienced a SQL exception (not a timeout)", ex);
            //}
        }

        protected int Execute(Func<DbConnection, int> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            
            using (var connection = OpenConnection())
            {
                var result = func(connection);
                return result;
            }
        }

        protected async Task<int> ExecuteAsync(Func<IDbConnection, Task<int>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            try
            {
                using (var connection = await OpenConnectionAsync())
                {
                    return await func(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(string.Format("{0}.ExecuteAsync() timed out", GetType().FullName));
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in ExecuteAsync()", ex);
            }
            //catch (SqlException ex)
            //{
            //    throw new Exception($"{GetType().FullName}.ExecuteAsync() experienced a SQL exception (not a timeout)", ex);
            //}
        }
    }
}
