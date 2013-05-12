using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using KLib.DataAccess.RepositoryImplementations;

namespace KLib.DataAccess
{
    public class Repository<TContext, TEntity> : IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        private readonly IRepository<TContext, TEntity> _implementation;

        public TContext Context { get; protected set; }

        public Repository(TContext context)
        {
            var contextType = typeof (TContext);
            Context = context;
            if (typeof (DbContext).IsAssignableFrom(contextType))
            {
                _implementation = new DbContextRepository<TContext, TEntity>(context);
            }
            else if (typeof (ObjectContext).IsAssignableFrom(contextType))
            {
                _implementation = new ObjectContextRepository<TContext, TEntity>(context);
            }
        }

        public virtual IQueryable<TEntity> All()
        {
            return _implementation.All();
        }

        public virtual TEntity FindByKey(params object[] keys)
        {
            return _implementation.FindByKey(keys);
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _implementation.First(predicate);
        }

        public virtual void Update(TEntity entity, IEnumerable<string> changeSet)
        {
            if (changeSet == null) throw new ArgumentException("No changed property found");            
            _implementation.Update(entity, changeSet);
        }

        public virtual void Replace(TEntity entity)
        {
            _implementation.Replace(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _implementation.Remove(entity);
        }

        public virtual IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments)
        {
            return _implementation.ExecuteQuery(sql, arguments);
        }
    }
}
