using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DataAccess
{
    /// <summary>
    /// Interface for Repository Factory
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Create a new repository from given TContext, TEntity and ContextManager
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager)
            where TEntity : class
            where TContext : class, IDisposable;

        /// <summary>
        /// Create a new repository from given contextType, entityType and ContextManager
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        object CreateRepository(Type contextType, Type entityType, ContextManager contextManager);

        /// <summary>
        ///  Create a new repository from given TRepository and ContextManager
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        TRepository CreateRepository<TRepository>(ContextManager contextManager);

        /// <summary>
        ///  Create a new repository from given repositoryType and ContextManager
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        object CreateRepository(Type repositoryType, ContextManager contextManager);

        /// <summary>
        /// Check to know the factory can create a repository from given contextType and entityType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        bool CanCreate(Type contextType, Type entityType);

        /// <summary>
        /// Check to know the factory can create a repository from given repositoryType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        bool CanCreate(Type repositoryType);

        /// <summary>
        /// Check to know the factory can create a repository from given TRepository
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        bool CanCreate<TRepository>();
    }
}
