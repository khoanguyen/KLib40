namespace KLib.DependencyInjection.Autofac
{
    public abstract class AutofacResolverFactoryBase : IResolverFactory
    {
        public IResolver CreateResolver()
        {
            return CreateResolverCore();
        }

        protected abstract AutofacResolver CreateResolverCore();
    }
}

