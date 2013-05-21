using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DependencyInjection.Ninject
{
    /// <summary>
    /// Base class for NinjectResolver Factory
    /// </summary>
    public abstract class NinjectResolverFactoryBase : IResolverFactory
    {
        /// <summary>
        /// Create a new Resolver
        /// </summary>
        /// <returns>Instance of NinjectResolver</returns>
        public IResolver CreateResolver()
        {
            var resolver = CreateResolverCore();
            InitResolver(resolver);
            return resolver;
        }

        /// <summary>
        /// Create a new Resolver
        /// </summary>
        /// <returns>Instance of NinjectResolver</returns>
        protected virtual NinjectResolver CreateResolverCore()
        {
            return new NinjectResolver();
        }

        /// <summary>
        /// Init the Resolver
        /// </summary>
        /// <param name="resolver"></param>
        protected virtual void InitResolver(NinjectResolver resolver)
        {
        }
    }
}
 