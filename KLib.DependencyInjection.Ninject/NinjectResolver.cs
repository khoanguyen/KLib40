using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning;

namespace KLib.DependencyInjection.Ninject
{
    /// <summary>
    /// Resolver with Ninject back-end
    /// </summary>
    public class NinjectResolver : IResolver
    {
        private readonly StandardKernel _kernel;

        /// <summary>
        /// Ninject Kernel
        /// </summary>
        public IKernel Kernel
        {
            get { return _kernel; }
        }

        /// <summary>
        /// Create new NinjectResolver from given Ninject modules
        /// </summary>
        /// <param name="modules">Ninject modules</param>
        public NinjectResolver(params INinjectModule[] modules)
        {
            _kernel = new StandardKernel(modules);
        }

        /// <summary>
        /// Create an instance of Service type T
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type T</returns>
        public T CreateInstance<T>(params object[] constructorArguments)
        {
            return (T)CreateInstance(typeof(T), constructorArguments);
        }

        /// <summary>
        /// Create an instance of given target Service type
        /// </summary>
        /// <param name="targetType">Service Type</param>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type</returns>
        public object CreateInstance(Type targetType, params object[] constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Length == 0)
                return _kernel.Get(targetType);

            var parameters = constructorArguments.OfType<ConstructorArgument>()
                                                .AsEnumerable<IParameter>()
                                                .ToArray();

            return _kernel.Get(targetType, parameters);

        }

        /// <summary>
        /// Resolve target object's dependencies       
        /// </summary>
        /// <param name="target">Target object</param>
        public void ResolveDependencies(object target)
        {
            _kernel.Inject(target);
        }

        /// <summary>
        /// Release target object's dependencies
        /// </summary>
        /// <param name="target"></param>
        public void Release(object target)
        {
            _kernel.Release(target);
        }

        /// <summary>
        /// Check if resolver can resolve the service type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool CanResolve<T>()
        {
            return CanResolve(typeof(T));
        }

        /// <summary>
        /// Check if resolver can resolve given service type
        /// </summary>
        /// <param name="targetType">service Type</param>
        /// <returns></returns>
        public bool CanResolve(Type targetType)
        {
            return _kernel.GetBindings(targetType).Count() > 0;
        }
    }
}
 