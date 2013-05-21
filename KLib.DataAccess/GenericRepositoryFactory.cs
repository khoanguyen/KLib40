using System;
using System.Diagnostics;
using KLib.DependencyInjection;

namespace KLib.DataAccess
{
    /// <summary>
    /// Repository Factory which provide generic Repository for accessing data       
    /// </summary>
    public class GenericRepositoryFactory : RepositoryFactoryBase
    {

        /// <summary>
        /// Create a new repository from given TContext, TEntity and ContextManager
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager)
        {
            return new Repository<TContext, TEntity>() { Context = contextManager.Context<TContext>() };
        }

        /// <summary>
        /// Create a new repository from given contextType, entityType and ContextManager
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override object CreateRepository(Type contextType, Type entityType, ContextManager contextManager)
        {
            Debug.Assert(CanCreate(contextType, entityType));
            var context = contextManager.Context(contextType);
            var repoType = typeof(Repository<,>).MakeGenericType(contextType, entityType);
            var result = Activator.CreateInstance(repoType);
            result.GetType().GetProperty("Context").SetValue(result, context, null);
            return result;
        }

        /// <summary>
        ///  Create a new repository from given TRepository and ContextManager
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override TRepository CreateRepository<TRepository>(ContextManager contextManager)
        {
            Debug.Assert(CanCreate<TRepository>());
            return (TRepository)CreateRepository(typeof(TRepository), contextManager);
        }

        /// <summary>
        ///  Create a new repository from given repositoryType and ContextManager
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override object CreateRepository(Type repositoryType, ContextManager contextManager)
        {
            Debug.Assert(CanCreate(repositoryType));
            var context = GetContextForRepository(repositoryType, contextManager);
            var result = Activator.CreateInstance(repositoryType);
            result.GetType().GetProperty("Context").SetValue(result, context, null);
            return result;
        }

        /// <summary>
        /// Check to know the factory can create a repository from given contextType and entityType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override bool CanCreate(Type contextType, Type entityType)
        {
            return true;
        }

        /// <summary>
        /// Check to know the factory can create a repository from given repositoryType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override bool CanCreate(Type repositoryType)
        {
            return repositoryType.IsGenericType &&
                   repositoryType.GetGenericTypeDefinition() == typeof(IRepository<,>) &&
                   null != TypeHelpers.GetBestMatch(repositoryType.GetConstructors(),
                                                    new object[] { new ContextManager() });
        }

        /// <summary>
        /// Check to know the factory can create a repository from given TRepository
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override bool CanCreate<TRepository>()
        {
            return CanCreate(typeof(TRepository));
        }
    }
}
