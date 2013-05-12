using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess
{
    public interface IRepository<out TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        TContext Context { get; }
        IQueryable<TEntity> All();
        TEntity FindByKey(params object[] keys);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity, IEnumerable<string> changeSet);
        void Replace(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments);
    }
}
