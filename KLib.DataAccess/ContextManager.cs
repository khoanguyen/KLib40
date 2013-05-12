﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Diagnostics;

namespace KLib.DataAccess
{
    public class ContextManager : IDisposable
    {
        private readonly Dictionary<Type, IDisposable> _contexts = new Dictionary<Type, IDisposable>();
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public static IRepositoryFactory RepositoryFactory { get; set; }           

        static ContextManager()
        {
            RepositoryFactory = new GenericRepositoryFactory();
        }

        public virtual IDisposable Context(Type contextType)
        {
            if (!_contexts.ContainsKey(contextType))
                _contexts[contextType] = Activator.CreateInstance(contextType) as IDisposable;

            return _contexts[contextType];
        }

        public virtual TContext Context<TContext>() where TContext : IDisposable
        {
            var contextType = typeof (TContext);
            if (!_contexts.ContainsKey(contextType))
                _contexts[contextType] = Activator.CreateInstance<TContext>();

            return (TContext) _contexts[contextType];
        }

        public virtual IRepository<TContext, TEntity> Repository<TContext, TEntity>()
            where TEntity : class
            where TContext : IDisposable
        {            
            return Repository(typeof(TContext), typeof(TEntity)) as IRepository<TContext, TEntity>;
        }

        public virtual object Repository(Type contextType, Type entityType) 
        {
            if (!_repositories.ContainsKey(entityType))
            {
                var repository = RepositoryFactory.CreateRepository(contextType, entityType, this);
                _repositories[entityType] = repository;
            }
            return _repositories[entityType];
        }

        public virtual void SaveChanges(Type contextType)
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

        public virtual void SaveChanges<TContext>() where TContext : IDisposable
        {
            SaveChanges(typeof (TContext));
        }

        public virtual void SaveAllChanges()
        {
            foreach (var contextType in _contexts.Keys) SaveChanges(contextType);
        }

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
    }
}
