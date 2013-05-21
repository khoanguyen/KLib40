using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KLib.DependencyInjection
{
    public static class TypeHelpers
    {
        /// <summary>
        /// Find the best match constructor for given set of arguments
        /// </summary>
        /// <param name="constructors">List of constructors</param>
        /// <param name="constructorArguments">Set of constructor arguments</param>
        /// <returns></returns>
        public static ConstructorInfo GetBestMatch(IEnumerable<ConstructorInfo> constructors,
                                                    object[] constructorArguments)
        {
            if (constructorArguments == null || constructorArguments.Length == 0)
                return constructors.FirstOrDefault(ci => ci.GetParameters().Length == 0);

            return constructors
                .FirstOrDefault(ci => ci.GetParameters().Count() == constructorArguments.Length &&
                                      !ci.GetParameters()
                                         .Where((t, i) => t.ParameterType != constructorArguments[i].GetType())
                                         .Any());
        }
    }
}
