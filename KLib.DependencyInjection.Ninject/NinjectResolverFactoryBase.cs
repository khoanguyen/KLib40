using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DependencyInjection.Ninject
{
    public abstract class NinjectResolverFactoryBase : IResolverFactory
    {
        public IResolver CreateResolver()
        {
            var resolver = CreateResolverCore();
            InitResolver(resolver);
            return resolver;
        }

        protected virtual NinjectResolver CreateResolverCore()
        {
            return new NinjectResolver();
        }

        protected virtual void InitResolver(NinjectResolver resolver)
        {
        }
    }
}
 