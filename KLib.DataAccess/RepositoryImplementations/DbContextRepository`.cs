using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess.RepositoryImplementations
{
    internal sealed class DbContextRepository<TContext, TEntity> : RepositoryImplementationBase<TEntity>, IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : class, IDisposable
    {
        private TContext _context;

        public TContext Context
        {
            get { return _context; }
            set
            {
                if (_context == value) return;
                Debug.Assert(_context is DbContext);
                _context = value;
                DbContext = _context as DbContext;
                DbSet = DbContext.Set<TEntity>();
            }
        }

        private DbContext DbContext { get; set; }

        internal DbSet<TEntity> DbSet { get; private set; }

        internal DbContextRepository(TContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable();
        }

        public TEntity FindByKey(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            EnsureEntityValidity(entity);
            DbSet.Add(entity);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] changeSet)
        {
            EnsureEntityValidity(entity);
            var entry = GetEntry(entity);
            foreach (var propEntry in GetChangeSet(changeSet)
                                        .Select(entry.Property)
                                        .Where(propEntry => propEntry != null))
            {
                propEntry.IsModified = true;
            }
        }

        public void ReplaceWith(TEntity entity)
        {
            EnsureEntityValidity(entity);
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

        private void EnsureEntityValidity(TEntity entity)
        {
            var entry = GetEntry(entity);
            var result = entry.GetValidationResult();

            if (!result.IsValid)
            {
                throw new DbEntityValidationException("Entity is not valid", new[] { result });
            }
        }
    }
}
