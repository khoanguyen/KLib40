using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DependencyInjection
{
    /// <summary>
    /// Factory for creating a resolver. Factory pattern allows customizing the creation of resolver.
    /// </summary>
    public interface IResolverFactory
    {
        /// <summary>
        /// Create a resolver
        /// </summary>
        /// <returns>A resolver instance</returns>
        IResolver CreateResolver();
    }
}
