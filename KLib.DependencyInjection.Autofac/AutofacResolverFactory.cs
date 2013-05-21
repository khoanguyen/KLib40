using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace KLib.DependencyInjection.Autofac
{
    /// <summary>
    /// Factory for creating Autofac Resolvers
    /// </summary>
    public class AutofacResolverFactory : AutofacResolverFactoryBase
    {
        private readonly Action<ContainerBuilder> _buildAction;

        /// <summary>
        /// Create new AutofacResolverFactory. Build action is for initializing the container
        /// </summary>
        /// <param name="buildAction">Build action for initializing the container</param>
        public AutofacResolverFactory(Action<ContainerBuilder> buildAction)
        {
            _buildAction = buildAction;
        }

        /// <summary>
        /// Create a Resolver
        /// </summary>
        /// <returns>Instance of Autofac Resolver</returns>
        protected override AutofacResolver CreateResolverCore()
        {
            var builder = new ContainerBuilder();
            _buildAction(builder);
            return new AutofacResolver(builder.Build());
        }
    }
}
