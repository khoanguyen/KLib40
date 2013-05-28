using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KLib.DataAccess
{
    /// <summary>
    /// Base class of Repository factory
    /// </summary>
    public abstract class RepositoryFactoryBase : IRepositoryFactory
    {
        /// <summary>
        /// Get base IRepository type of the derived type if it inherits from the hierarchy of IRepository
        /// </summary>
        /// <param name="derivedType"></param>
        /// <returns></returns>
        protected virtual Type GetIRepositoryType(Type derivedType)
        {
            if (derivedType.IsGenericType && derivedType.GetGenericTypeDefinition() == typeof(IRepository<,>))
                return derivedType;
            var repoType = typeof(IRepository<,>);
            var result = derivedType.GetInterfaces()
                                    .FirstOrDefault(interfaze => interfaze.IsGenericType &&
                                                                 interfaze.GetGenericTypeDefinition() == repoType);
            if (result == null)
                throw new ArgumentException(string.Format("{0} does not implement IRepository interface", derivedType.Name));
            return result;
        }

        /// <summary>
        /// Get context for give repositoryType from contextManager
        /// </summary>
        /// <param name="repositoryType"></param>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        protected IDisposable GetContextForRepository(Type repositoryType, ContextManager contextManager)
        {
            Debug.Assert(CanCreate(repositoryType));
            var irepoType = GetIRepositoryType(repositoryType);
            Debug.Assert(irepoType.GetGenericArguments().Length == 2);
            return contextManager.Context(irepoType.GetGenericArguments()[0]);
        }


        #region Abstract methods of IRepositoryFactory
        public abstract IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager)
            where TContext : class, IDisposable
            where TEntity : class;

        public abstract object CreateRepository(Type contextType, Type entityType, ContextManager contextManager);

        public abstract TRepository CreateRepository<TRepository>(ContextManager contextManager);

        public abstract object CreateRepository(Type repositoryType, ContextManager contextManager);

        public abstract bool CanCreate(Type contextType, Type entityType);

        public abstract bool CanCreate(Type repositoryType);

        public abstract bool CanCreate<TRepository>();
        #endregion
    }
}
