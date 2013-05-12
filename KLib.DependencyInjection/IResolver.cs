using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DependencyInjection
{
    public interface IResolver
    {
        T CreateInstance<T>(params object[] constructorArguments);
        object CreateInstance(Type targetType, params object[] constructorArguments);
        void ResolveDependencies(object target);
        void Release(object target);
    }
}
