using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;

namespace KLib.DependencyInjection.Autofac
{
    /// <summary>
    /// Resolver with Autofac back-end
    /// </summary>
    public class AutofacResolver : IResolver
    {
        private readonly IContainer _container;

        /// <summary>
        /// Create new Resolver from Autofac container
        /// </summary>
        /// <param name="container"></param>
        public AutofacResolver(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Create an instance of Service type T
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type T</returns>
        public T CreateInstance<T>(params object[] constructorArguments)
        {
            return _container.Resolve<T>(GetConstructorParameters(typeof(T), constructorArguments));
        }

        /// <summary>
        /// Create an instance of given target Service type
        /// </summary>
        /// <param name="targetType">Service Type</param>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type</returns>
        public object CreateInstance(Type targetType, params object[] constructorArguments)
        {
            return _container.Resolve(targetType, GetConstructorParameters(targetType, constructorArguments));
        }

        /// <summary>
        /// Resolve target object's dependencies       
        /// </summary>
        /// <param name="target">Target object</param>
        public void ResolveDependencies(object target)
        {
            _container.InjectProperties(target);
        }

        /// <summary>
        /// Release target object's dependencies
        /// </summary>
        /// <param name="target"></param>
        public void Release(object target)
        {
            // Do nothing
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
            return _container.IsRegistered(targetType);
        }

        /// <summary>
        /// Get the list of constructor parameter of the given set of arguments 
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <param name="constructorArguments">Set of arguments</param>
        /// <returns>list of constructor parameter</returns>
        private IEnumerable<Parameter> GetConstructorParameters(Type serviceType, object[] constructorArguments)
        {

            var registry = _container.ComponentRegistry
                                     .Registrations
                                     .FirstOrDefault(r => r.Services.OfType<TypedService>().Any(s => s.ServiceType == serviceType));
            if (registry == null)
                throw new InvalidOperationException(string.Format("Could not find any registration for {0}", serviceType.Name));

            var targetType = registry.Activator.LimitType;

            if (constructorArguments == null || constructorArguments.Length == 0)
                return new Parameter[0];
            var constructors = targetType.GetConstructors();
            var bestMatch = TypeHelpers.GetBestMatch(constructors, constructorArguments);
            if (bestMatch == null)
                throw new InvalidOperationException("Could not find a best match constructor for input arguments");
            return bestMatch.GetParameters()
                            .Select<ParameterInfo, Parameter>(
                                (pi, i) => new NamedParameter(pi.Name, constructorArguments[i]))
                            .ToArray();
        }
    }
}
