using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DataAccess
{
    public interface IRepositoryFactory
    {
        IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager) 
            where TEntity : class
            where TContext : IDisposable;

        object CreateRepository(Type contextType, Type entityType, ContextManager contextManager);
    }
}
