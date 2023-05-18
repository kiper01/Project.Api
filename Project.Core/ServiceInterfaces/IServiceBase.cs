using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Project.Core.OperationInterfaces
{
    public interface IServiceBase
    {
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate)
             where TEntity : class;

        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
             where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>()
             where TEntity : class;

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
             where TEntity : class;

        TEntity Add<TEntity>(TEntity entity)
            where TEntity : class;

        Task<TEntity> AddAsync<TEntity>(TEntity entity)
             where TEntity : class;

        void AddRange<TEntity>(IEnumerable<TEntity> entity)
            where TEntity : class;

        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entity)
            where TEntity : class;

        void Delete<TEntity>(int id)
            where TEntity : class;

        void Delete<TEntity>(Guid id)
          where TEntity : class;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class;

        void DeleteRange<TEntity>(IEnumerable<TEntity> entity)
          where TEntity : class;


        TEntity Get<TEntity>(int id)
            where TEntity : class;

        TEntity Get<TEntity>(Guid id)
             where TEntity : class;

        Task<TEntity> GetAsync<TEntity>(int id)
             where TEntity : class;

        Task<TEntity> GetAsync<TEntity>(Guid id)
            where TEntity : class;

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class;

        void UpdateRange<TEntity>(IEnumerable<TEntity> entity)
          where TEntity : class;

        IDbContextTransaction BeginTransaction();

        IEnumerable<dynamic> GetObjectsToSQL(string sql);

    }
}
