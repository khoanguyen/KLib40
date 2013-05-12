using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DataAccess
{
    public class GenericRepositoryFactory : IRepositoryFactory
    {
        public virtual IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager) where TContext : IDisposable where TEntity : class
        {
            return new Repository<TContext, TEntity>(contextManager.Context<TContext>());
        }

        public virtual object CreateRepository(Type contextType, Type entityType, ContextManager contextManager)
        {
            var context = contextManager.Context(contextType);
            var repoType = typeof (Repository<,>).MakeGenericType(contextType, entityType);
            return Activator.CreateInstance(repoType, context);
        }
    }
}
