namespace KLib.DependencyInjection.Autofac
{
    /// <summary>
    /// Base class for AutofacResolver Factory
    /// </summary>
    public abstract class AutofacResolverFactoryBase : IResolverFactory
    {
        /// <summary>
        /// Create a new Resolver
        /// </summary>
        /// <returns>Instance of AutofacResolver</returns>
        public IResolver CreateResolver()
        {
            return CreateResolverCore();
        }

        /// <summary>
        /// Create new Resolver core
        /// </summary>
        /// <returns>Instance of AutofacResolver</returns>
        protected abstract AutofacResolver CreateResolverCore();
    }
}

