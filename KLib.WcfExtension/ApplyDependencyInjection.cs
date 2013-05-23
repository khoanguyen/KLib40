using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using KLib.DependencyInjection;

namespace KLib.WcfExtension
{
    public sealed class ApplyDependencyInjection : Attribute, IServiceBehavior
    {
        private readonly IResolverFactory _resolverFactory;

        public ApplyDependencyInjection(Type resolverFactoryType)
        {
            EnsureValidType(resolverFactoryType);
            _resolverFactory = (IResolverFactory)Activator.CreateInstance(resolverFactoryType);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {            
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Apply DI InstanceContext initializer on endpoints
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                foreach (var epDispatcher in dispatcher.Endpoints)
                {
                    var diInstanceContextInitializer = new DiInstanceContextInitializer(_resolverFactory);
                    epDispatcher.DispatchRuntime
                                .InstanceContextInitializers
                                .Add(diInstanceContextInitializer);
                }
            }
        }

        private static void EnsureValidType(Type factoryType)
        {
            if (factoryType == null) throw new ArgumentNullException("factoryType");
            if (!typeof (IResolverFactory).IsAssignableFrom(factoryType))
                throw new ArgumentException("Expecting a Resolver factory type which implements IResolverFactory");
        }
    }
}
