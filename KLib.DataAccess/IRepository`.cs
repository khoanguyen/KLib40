using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess
{
    /// <summary>
    /// Interface of Repository
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        /// <summary>
        /// Associated context, this could be DbContext or ObjectContext
        /// </summary>
        TContext Context { get; set; }

        /// <summary>
        /// Get queryable object which could return all object in the database
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Find object by primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity FindByKey(params object[] keys);

        /// <summary>
        /// Get the first object which satisfies the query condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity First(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

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
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] changeSet);

        /// <summary>
        /// Replace an existing entity with the new data
        /// </summary>
        /// <param name="entity"></param>
        void ReplaceWith(TEntity entity);

        /// <summary>
        /// Remove the given entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Execute a raw query against the database
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments);
    }
}
