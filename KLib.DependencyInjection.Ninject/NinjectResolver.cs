using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace KLib.DependencyInjection.Ninject
{
    public class NinjectResolver : IResolver
    {
        private readonly IKernel _kernel;        

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public NinjectResolver(params INinjectModule[] modules)
        {
            _kernel = new StandardKernel(modules);
        }

        public T CreateInstance<T>(params object[] constructorArguments)
        {
            return _kernel.Get<T>(GetConstructorParameters(typeof (T), constructorArguments));
        }

        public object CreateInstance(Type targetType, params object[] constructorArguments)
        {
            return _kernel.Get(targetType, GetConstructorParameters(targetType, constructorArguments));
        }

        public void ResolveDependencies(object target)
        {
            _kernel.Inject(target);
        }

        public void Release(object target)
        {
            _kernel.Release(target);
        }

        private static IParameter[] GetConstructorParameters(Type targetType, object[] constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Length == 0)
                return null;
            var constructors = targetType.GetConstructors();
            var bestMatch = TypeHelpers.GetBestMatch(constructors, constructorArguments);
            if (bestMatch == null)
                throw new InvalidOperationException("Could not find a best match constructor for input arguments");
            return bestMatch.GetParameters()
                            .Select<ParameterInfo, IParameter>(
                                (pi, i) => new ConstructorArgument(pi.Name, constructorArguments[i]))
                            .ToArray();
        }
    }
}
 