using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;

namespace KLib.DependencyInjection.Ninject
{
    public class NinjectResolverFactory : NinjectResolverFactoryBase
    {
        private readonly INinjectModule[] _modules;

        public NinjectResolverFactory(params INinjectModule[] modules)
        {
            _modules = modules;
        }

        protected override NinjectResolver CreateResolverCore()
        {
            return new NinjectResolver(_modules);
        }
    }
}
 