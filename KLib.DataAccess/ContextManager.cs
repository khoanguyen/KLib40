using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Diagnostics;

namespace KLib.DataAccess
{
    /// <summary>
    /// <para>
    /// Context Manager works as a Distributed Unit Of Work.
    /// </para>
    /// <para>
    /// It is responsible for creating and managing DbContexts, ObjectContexts, Repositories
    /// and automatically wire the dependencies between these contexts and repositories,
    /// so that users do not need to care about the creation of the infrastructure.
    /// </para>
    /// </summary>
    public class ContextManager : IDisposable
    {
        private readonly Dictionary<Type, IDisposable> _contexts = new Dictionary<Type, IDisposable>();
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public static IList<IRepositoryFactory> RepositoryFactories { get; private set; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static ContextManager()
        {
            // Initialize the chain of Repository Factories
            RepositoryFactories = new List<IRepositoryFactory>();
            RepositoryFactories.Add(new GenericRepositoryFactory());
        }

        /// <summary>
        /// Get a DbContex/ObjectContext instance of given context type
        /// </summary>
        /// <param name="contextType"></param>
        /// <returns></returns>
        public virtual IDisposable Context(Type contextType)
        {
            if (!_contexts.ContainsKey(contextType))
                _contexts[contextType] = Activator.CreateInstance(contextType) as IDisposable;

            return _contexts[contextType];
        }

        /// <summary>
        /// Get a DbContex/ObjectContext instance of type TContext
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public virtual TContext Context<TContext>() where TContext : IDisposable
        {
            var contextType = typeof(TContext);
            if (!_contexts.ContainsKey(contextType))
                _contexts[contextType] = Activator.CreateInstance<TContext>();

            return (TContext)_contexts[contextType];
        }

        /// <summary>
        /// Get a DbContex/ObjectContext instance of type TContext
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        public virtual TRepository Repository<TRepository>()
            where TRepository : class
        {
            var repoType = typeof(TRepository);
            if (!_repositories.ContainsKey(repoType))
            {
                var factory = GetRepositoryFactory(repoType);
                if (factory == null)
                    throw new InvalidOperationException("Could not find appropriate factory");
                Debug.Assert(factory.CanCreate<TRepository>());
                _repositories[repoType] = factory.CreateRepository<TRepository>(this);
            }
            return (TRepository)_repositories[repoType];
        }

        /// <summary>
        /// Get a Repository for type TEntity which has Context of type TContext
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual IRepository<TContext, TEntity> Repository<TContext, TEntity>()
            where TEntity : class
            where TContext : class, IDisposable
        {
            return Repository(typeof(TContext), typeof(TEntity)) as IRepository<TContext, TEntity>;
        }

        /// <summary>
        /// Get a Repository for entityType which has Context of contextType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public virtual object Repository(Type contextType, Type entityType)
        {
            var repoType = typeof(IRepository<,>).MakeGenericType(contextType, entityType);
            if (!_repositories.ContainsKey(repoType))
            {
                var factory = GetRepositoryFactory(contextType, entityType);
                if (factory == null)
                    throw new InvalidOperationException("Could not find appropriate factory");
                Debug.Assert(factory.CanCreate(contextType, entityType));
                _repositories[repoType] = factory.CreateRepository(contextType, entityType, this); ;
            }
            return _repositories[repoType];
        }

        /// <summary>
        /// Save pending changes on a specific context identified by contextType
        /// </summary>
        /// <param name="contextType"></param>
        public virtual void SaveChanges(Type contextType)
        {
            try
            {
                if (!_contexts.ContainsKey(contextType)) return;
                var context = _contexts[contextType];
                if (context is DbContext)
                {
                    var dbContext = _contexts[contextType] as DbContext;
                    Debug.Assert(dbContext != null, "dbContext != null");
                    dbContext.ChangeTracker.DetectChanges();
                    dbContext.SaveChanges();
                }
                else if (context is ObjectContext)
                {
                    var objContext = _contexts[contextType] as ObjectContext;
                    Debug.Assert(objContext != null, "objContext != null");
                    objContext.DetectChanges();
                    objContext.SaveChanges();
                    objContext.AcceptAllChanges();
                }
            }
            catch (Exception ex)
            {
                throw new SaveChangesException(ex);
            }
        }

        /// <summary>
        /// Save pending changes on a specific context identified by TContext
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        public virtual void SaveChanges<TContext>() where TContext : IDisposable
        {
            SaveChanges(typeof(TContext));
        }

        /// <summary>
        /// Save all changes on all the contexts being managed by this ContextManager
        /// </summary>
        public virtual void SaveAllChanges()
        {
            foreach (var contextType in _contexts.Keys) SaveChanges(contextType);
        }

        /// <summary>
        /// Dispose current ContextManager
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var context in _contexts)
            {
                try
                {
                    context.Value.Dispose();
                }
                catch (Exception)
                {
                    // Do nothing
                }
            }
        }

        /// <summary>
        /// Get RepositoryFactory which can create a repository which satisfy the contextType and entityType
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private IRepositoryFactory GetRepositoryFactory(Type contextType, Type entityType)
        {
            for (var i = RepositoryFactories.Count - 1; i >= 0; i--)
            {
                var factory = RepositoryFactories[i];
                if (factory.CanCreate(contextType, entityType))
                {
                    return factory;
                }
            }

            return null;
        }

        /// <summary>
        /// Get RepositoryFactory which can create a repository of repositoryType
        /// </summary>
        /// <param name="repositoryType"></param>
        /// <returns></returns>
        private IRepositoryFactory GetRepositoryFactory(Type repositoryType)
        {
            for (var i = RepositoryFactories.Count - 1; i >= 0; i--)
            {
                var factory = RepositoryFactories[i];
                if (factory.CanCreate(repositoryType))
                {
                    return factory;
                }
            }

            return null;
        }
    }
}
