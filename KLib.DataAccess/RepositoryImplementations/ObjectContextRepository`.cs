using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess.RepositoryImplementations
{
    internal sealed class ObjectContextRepository<TContext, TEntity> : RepositoryImplementationBase<TEntity>, IRepository<TContext, TEntity>
        where TEntity : class
        where TContext : IDisposable
    {
        public TContext Context { get; set; }

        private ObjectContext ObjContext { get; set; }

        internal ObjectSet<TEntity> ObjectSet { get; private set; }

        private string QualifiedEntitySetName { get; set; }

        internal ObjectContextRepository(TContext context)
        {
            Debug.Assert(context is ObjectContext);
            Context = context;
            ObjContext = Context as ObjectContext;
            ObjectSet = ObjContext.CreateObjectSet<TEntity>();
            QualifiedEntitySetName = string.Format("{0}.{1}", ObjContext.DefaultContainerName, ObjectSet.EntitySet.Name);
        }

        public IQueryable<TEntity> All()
        {
            return ObjectSet;
        }

        public TEntity FindByKey(params object[] keys)
        {
            var edmMembers = ObjectSet.EntitySet.ElementType.KeyMembers;
            if (keys.Length != edmMembers.Count)
                throw new ArgumentException("Input values does not match the number of entity primary key");

            var i = 0;
            var keyMemebers = edmMembers.Select(k =>
            {
                var km = new EntityKeyMember(k.Name, keys[i++]);
                return km;
            }).ToArray();

            var entityKey = new EntityKey(QualifiedEntitySetName, keyMemebers);
            object result;
            ObjContext.TryGetObjectByKey(entityKey, out result);
            return result as TEntity;
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            EnsureEntityValidity(entity);
            ObjectSet.AddObject(entity);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] changeSet)
        {
            var entry = GetEntityEntry(entity);
            foreach (var propName in GetChangeSet(changeSet))
                entry.SetModifiedProperty(propName);
        }

        public void ReplaceWith(TEntity entity)
        {
            var entry = GetEntityEntry(entity);
            entry.ChangeState(EntityState.Modified);
        }

        public void Remove(TEntity entity)
        {
            var entry = GetEntityEntry(entity);
            entry.Delete();
        }

        public IEnumerable<TEntity> ExecuteQuery(string sql, params object[] arguments)
        {
            return ObjContext.ExecuteStoreQuery<TEntity>(sql, arguments);
        }

        private ObjectStateEntry GetEntityEntry(TEntity entity)
        {
            ObjectStateEntry entry;
            if (!ObjContext.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                ObjectSet.Attach(entity);
                entry = ObjContext.ObjectStateManager.GetObjectStateEntry(entity);
            }
            return entry;
        }

        private void EnsureEntityValidity(TEntity entity)
        {
            // Do nothing looking for a way to validate ObjectContext's entites
            // At the moment just expecting an Exception when saving changes
        }
    }
}
