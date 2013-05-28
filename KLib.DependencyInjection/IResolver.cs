using System;

namespace KLib.DependencyInjection
{
    /// <summary>
    /// <para>
    /// Resolver is abstraction layer for Dependency Inject Pattern
    /// </para>    
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Create an instance of Service type T
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type T</returns>
        T CreateInstance<T>(params object[] constructorArguments);

        /// <summary>
        /// Create an instance of given target Service type
        /// </summary>
        /// <param name="targetType">Service Type</param>
        /// <param name="constructorArguments">Optional constructor parameters</param>
        /// <returns>Instance of service type</returns>
        object CreateInstance(Type targetType, params object[] constructorArguments);

        /// <summary>
        /// Resolve target object's dependencies       
        /// </summary>
        /// <param name="target">Target object</param>
        void ResolveDependencies(object target);

        /// <summary>
        /// Release target object's dependencies
        /// </summary>
        /// <param name="target"></param>
        void Release(object target);

        /// <summary>
        /// Check if resolver can resolve the service type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool CanResolve<T>();

        /// <summary>
        /// Check if resolver can resolve given service type
        /// </summary>
        /// <param name="targetType">service Type</param>
        /// <returns></returns>
        bool CanResolve(Type targetType);
    }
}
