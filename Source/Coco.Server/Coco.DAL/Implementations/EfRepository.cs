﻿using Coco.Contract;
using Coco.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Coco.DAL.Implementations
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly ContentDbContext _dbContext;

        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> DbSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = _dbContext.Set<TEntity>();
                }

                return _dbSet;
            }
        }
        #endregion

        #region Ctor

        public EfRepository(ContentDbContext context)
        {
            _dbContext = context;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Get()
        {
            return DbSet;
        }

        /// <summary>
        /// Get entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Where(filter);
        }

        /// <summary>
        /// Get entities async
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <summary>
        /// Get entities by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DbSet.Where(filter).ToListAsync();
        }

        /// <summary>
        /// Get first or default
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault()
        {
            return DbSet.FirstOrDefault();
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => DbSet;

        /// <summary>
        /// Get first or default async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await DbSet.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get first or default by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.FirstOrDefault(filter);
        }

        /// <summary>
        /// Get first or default by filter async
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await DbSet.FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual TEntity Find(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> FindAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Add(entity);
        }

        /// <summary>
        /// Add entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Add(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Update(entity);
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbSet.Remove(entity);
        }

        /// <summary>
        /// Delete by id
        /// </summary>
        /// <param name="id">Entity Id</param>
        public virtual void Delete(int id)
        {
            if (id <= 0)
            {
                throw new MissingPrimaryKeyException(nameof(id));
            }

            TEntity entity = Find(id);

            Delete(entity);
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            DbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateByName(TEntity entity, object value, string propertyName, bool isIgnoreCase = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var type = entity.GetType();
            PropertyInfo propertyInfo;
            if (isIgnoreCase)
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            else
            {
                propertyInfo = type.GetProperty(propertyName);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyType), null);

            DbSet.Update(entity);
        }
        #endregion
    }
}
