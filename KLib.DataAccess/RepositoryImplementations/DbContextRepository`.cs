using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess.RepositoryImplementations
{
    internal sealed class DbContextRepository<TContext, TEntity> : IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        public TContext Context { get; private set; }

        private DbContext DbContext { get; set; }

        internal DbSet<TEntity> DbSet { get; private set; }

        internal DbContextRepository(TContext context)
        {
            Debug.Assert(context is DbContext);
            Context = context;
            DbContext = Context as DbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return DbSet;
        }

        public TEntity FindByKey(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public void Update(TEntity entity, IEnumerable<string> changeSet)
        {
            var entry = GetEntry(entity);
            foreach (var propEntry in changeSet.Select(entry.Property)
                                               .Where(propEntry => propEntry != null))
            {
                propEntry.IsModified = true;
            }
        }

        public void Replace(TEntity entity)
        {
            var entry = GetEntry(entity);
            entry.State = System.Data.EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            var entry = GetEntry(entity);
            entry.State = System.Data.EntityState.Deleted;
        }

        public IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments)
        {
            return DbSet.SqlQuery(sql, arguments);
        }

        private DbEntityEntry<TEntity> GetEntry(TEntity entity)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == System.Data.EntityState.Detached)
                DbSet.Attach(entity);
            return entry;
        } 
    }
}
