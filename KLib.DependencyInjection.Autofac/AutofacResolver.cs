using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;

namespace KLib.DependencyInjection.Autofac
{
    public class AutofacResolver : IResolver
    {
        private readonly IContainer _container;

        public AutofacResolver(IContainer container)
        {
            _container = container;
        }

        public T CreateInstance<T>(params object[] constructorArguments)
        {
            return _container.Resolve<T>(GetConstructorParameters(typeof (T), constructorArguments));
        }

        public object CreateInstance(Type targetType, params object[] constructorArguments)
        {
            return _container.Resolve(targetType, GetConstructorParameters(targetType, constructorArguments));
        }

        public void ResolveDependencies(object target)
        {
            _container.InjectProperties(target);
        }

        public void Release(object target)
        {
            // Do nothing
        }

        private static IEnumerable<Parameter> GetConstructorParameters(Type targetType, object[] constructorArguments)
        {
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
