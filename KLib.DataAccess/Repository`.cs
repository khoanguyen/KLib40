using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using KLib.DataAccess.RepositoryImplementations;

namespace KLib.DataAccess
{
    /// <summary>
    /// Generic Repository which can be used with any type of Context and Entity
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TContext, TEntity> : IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        private IRepository<TContext, TEntity> _implementation;
        private TContext _context;

        /// <summary>
        /// Associated context, this could be DbContext or ObjectContext
        /// </summary>
        public virtual TContext Context
        {
            get { return _context; }
            set
            {
                var contextType = typeof(TContext);
                _context = value;
                if (typeof(DbContext).IsAssignableFrom(contextType))
                {
                    _implementation = new DbContextRepository<TContext, TEntity>(_context);
                }
                else if (typeof(ObjectContext).IsAssignableFrom(contextType))
                {
                    _implementation = new ObjectContextRepository<TContext, TEntity>(_context);
                }
            }
        }

        /// <summary>
        /// Get queryable object which could return all object in the database
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> All()
        {
            return _implementation.All();
        }

        /// <summary>
        /// Find object by primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual TEntity FindByKey(params object[] keys)
        {
            return _implementation.FindByKey(keys);
        }

        /// <summary>
        /// Get the first object which satisfies the query condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _implementation.First(predicate);
        }

        /// <summary>
        /// <para>
        /// Update an existing entity 
        /// </para>
        /// <para>
        /// This method need a change set which indicates modified properties 
        /// </para>
        /// </summary>
        /// <param name="entity">updated entity</param>
        /// <param name="changeSet"></param>
        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] changeSet)
        {
            _implementation.Update(entity, changeSet);
        }

        /// <summary>
        /// Replace an existing entity with the new data
        /// </summary>
        /// <param name="entity"></param>
        public virtual void ReplaceWith(TEntity entity)
        {
            _implementation.ReplaceWith(entity);
        }

        /// <summary>
        /// Remove the given entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(TEntity entity)
        {
            _implementation.Remove(entity);
        }

        /// <summary>
        /// Execute a raw query against the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments)
        {
            return _implementation.ExecuteQuery(sql, arguments);
        }

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            _implementation.Add(entity);
        }



    }
}
