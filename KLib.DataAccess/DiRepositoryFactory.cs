using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.DependencyInjection;

namespace KLib.DataAccess
{
    public class DiRepositoryFactory : IRepositoryFactory
    {        
        private readonly IResolver _resolver;

        public DiRepositoryFactory(IResolver resolver)
        {
            _resolver = resolver;
        }

        public virtual IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager)
            where TContext : IDisposable
            where TEntity : class
        {
            return _resolver.CreateInstance<IRepository<TContext, TEntity>>(contextManager.GetContext<TContext>());
        }

        public virtual object CreateRepository(Type contextType, Type entityType, ContextManager contextManager)
        {
            var repositoryType = typeof(IRepository<,>).MakeGenericType(contextType, entityType);
            return _resolver.CreateInstance(repositoryType, contextManager.GetContext(contextType));
        }
        
    }
}
