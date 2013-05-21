using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;

namespace KLib.DependencyInjection.Ninject
{
    /// <summary>
    /// Factory for creating Ninject Resolvers
    /// </summary>
    public class NinjectResolverFactory : NinjectResolverFactoryBase
    {
        private readonly INinjectModule[] _modules;

        /// <summary>
        /// Create new NinjectResolverFactory. Build action is for initializing the container
        /// </summary>
        /// <param name="buildAction">Build action for initializing the container</param>
        public NinjectResolverFactory(params INinjectModule[] modules)
        {
            _modules = modules;
        }

        /// <summary>
        /// Create a Resolver
        /// </summary>
        /// <returns>Instance of Autofac Resolver</returns>
        protected override NinjectResolver CreateResolverCore()
        {
            return new NinjectResolver(_modules);
        }
    }
}
 