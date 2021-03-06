﻿using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Camino.Data.Contracts
{
    public interface IBaseDataProvider
    {
        ITable<TEntity> GetTable<TEntity>() where TEntity : class;
        void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        int Insert<TEntity>(TEntity entity);
        Task<int> InsertAsync<TEntity>(TEntity entity);
        object InsertWithIdentity<TEntity>(TEntity entity);
        Task<object> InsertWithIdentityAsync<TEntity>(TEntity entity);
        int InsertWithInt32Identity<TEntity>(TEntity entity);
        Task<int> InsertWithInt32IdentityAsync<TEntity>(TEntity entity);
        Task<long> InsertWithInt64IdentityAsync<TEntity>(TEntity entity);
        long InsertWithInt64Identity<TEntity>(TEntity entity);
        void Update<TEntity>(TEntity entity);
        Task UpdateAsync<TEntity>(TEntity entity);
        void Delete<TEntity>(TEntity entity);
        Task DeleteAsync<TEntity>(TEntity entity);
        void DeleteRange<TEntity>(IQueryable<TEntity> entities);
        Task DeleteRangeAsync<TEntity>(IQueryable<TEntity> entities);
        void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
        Task UpdateByNameAsync<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
        DataConnectionTransaction BeginTransaction();
        Task<DataConnectionTransaction> BeginTransactionAsync();
        void CommitTransaction();
        Task CommitTransactionAsync();
        void RollbackTransaction();
        Task RollbackTransactionAsync();
        bool IsDatabaseExist();
        DataConnection CreateDataConnection();
        SqlConnectionStringBuilder GetConnectionStringBuilder();
        IList<string> GetCommandsFromScript(string sql);
    }
}
