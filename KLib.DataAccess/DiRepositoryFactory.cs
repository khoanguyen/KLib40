using System;
using KLib.DependencyInjection;

namespace KLib.DataAccess
{
    /// <summary>
    /// Repository Factory which supports Dependency Injection        
    /// </summary>
    public sealed class DiRepositoryFactory : RepositoryFactoryBase
    {
        private readonly IResolver _resolver;

        /// <summary>
        /// Create new repository factory from given resolver
        /// </summary>
        /// <param name="resolver"></param>
        public DiRepositoryFactory(IResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Create a new repository from given TContext, TEntity and ContextManager
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override IRepository<TContext, TEntity> CreateRepository<TContext, TEntity>(ContextManager contextManager)
        {
            var result = _resolver.CreateInstance<IRepository<TContext, TEntity>>();
            result.Context = contextManager.Context<TContext>();
            return result;
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
            var repositoryType = typeof(IRepository<,>).MakeGenericType(contextType, entityType);
            var result = _resolver.CreateInstance(repositoryType);
            result.GetType().GetProperty("Context").SetValue(result, contextManager.Context(contextType), null);
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
            var repositoryType = typeof(TRepository);
            return (TRepository)CreateRepository(repositoryType, contextManager);
        }

        /// <summary>
        ///  Create a new repository from given repositoryType and ContextManager
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <param name="contextManager"></param>
        /// <returns></returns>
        public override object CreateRepository(Type repositoryType, ContextManager contextManager)
        {
            var result = _resolver.CreateInstance(repositoryType);
            result.GetType().GetProperty("Context").SetValue(result, GetContextForRepository(repositoryType, contextManager), null);
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

            var repositoryType = typeof(IRepository<,>).MakeGenericType(contextType, entityType);
            return _resolver.CanResolve(repositoryType);
        }

        /// <summary>
        /// Check to know the factory can create a repository from given repositoryType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override bool CanCreate(Type repositoryType)
        {
            return _resolver.CanResolve(repositoryType);
        }

        /// <summary>
        /// Check to know the factory can create a repository from given TRepository
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public override bool CanCreate<TRepository>()
        {
            return _resolver.CanResolve<TRepository>();
        }
    }
}
