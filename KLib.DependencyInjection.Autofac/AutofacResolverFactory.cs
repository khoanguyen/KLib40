using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace KLib.DependencyInjection.Autofac
{
    public class AutofacResolverFactory : AutofacResolverFactoryBase
    {
        private readonly Action<ContainerBuilder> _buildAction;

        public AutofacResolverFactory(Action<ContainerBuilder> buildAction)
        {
            _buildAction = buildAction;
        }

        protected override AutofacResolver CreateResolverCore()
        {
            var builder = new ContainerBuilder();
            _buildAction(builder);
            return new AutofacResolver(builder.Build());
        }
    }
}
