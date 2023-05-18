using Microsoft.EntityFrameworkCore.Storage;
using Project.Core.OperationInterfaces;
using Project.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class ServiceBase : IServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate)
    where TEntity : class
        {
            return _unitOfWork.Repository<TEntity>().Find(predicate);
        }

        public Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _unitOfWork.Repository<TEntity>().FindAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : class
        {
            return _unitOfWork.Repository<TEntity>().GetAll();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
           where TEntity : class
        {
            return await _unitOfWork.Repository<TEntity>().GetAllAsync();
        }

        public TEntity Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            var outEntity = _unitOfWork.Repository<TEntity>().Add(entity);
            _unitOfWork.Complete();
            return outEntity;
        }
        public async Task<TEntity> AddAsync<TEntity>(TEntity entity)
          where TEntity : class
        {
            var outEntity = await _unitOfWork.Repository<TEntity>().AddAsync(entity);
            _unitOfWork.Complete();
            return outEntity;
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.Repository<TEntity>().AddRange(entities);
            _unitOfWork.Complete();
        }

        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            await _unitOfWork.Repository<TEntity>().AddRangeAsync(entities);
            _unitOfWork.Complete();
        }

        public void Delete<TEntity>(int id)
            where TEntity : class
        {
            var entity = Get<TEntity>(id);
            if (entity == null) return;

            _unitOfWork.Repository<TEntity>().Remove(entity);
            _unitOfWork.Complete();
        }

        public void Delete<TEntity>(Guid id)
    where TEntity : class
        {
            var entity = Get<TEntity>(id);
            if (entity == null) return;

            _unitOfWork.Repository<TEntity>().Remove(entity);
            _unitOfWork.Complete();
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity == null) return;

            _unitOfWork.Repository<TEntity>().Remove(entity);
            _unitOfWork.Complete();
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.Repository<TEntity>().RemoveRange(entities);
            _unitOfWork.Complete();
        }

        public TEntity Get<TEntity>(int id)
            where TEntity : class
        {
            return _unitOfWork.Repository<TEntity>().Get(id);
        }

        public async Task<TEntity> GetAsync<TEntity>(int id)
     where TEntity : class
        {
            return await _unitOfWork.Repository<TEntity>().GetAsync(id);
        }


        public TEntity Get<TEntity>(Guid id)
           where TEntity : class
        {
            return _unitOfWork.Repository<TEntity>().Get(id);
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid id)
     where TEntity : class
        {
            return await _unitOfWork.Repository<TEntity>().GetAsync(id);
        }


        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            var outEntity = _unitOfWork.Repository<TEntity>().Update(entity);
            _unitOfWork.Complete();
            return outEntity;
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.Repository<TEntity>().UpdateRange(entities);
            _unitOfWork.Complete();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _unitOfWork.BeginTransaction();
        }
        public IEnumerable<dynamic> GetObjectsToSQL(string sql)
        {
            return _unitOfWork.GetObjectsToSQL(sql);
        }
    }
}
