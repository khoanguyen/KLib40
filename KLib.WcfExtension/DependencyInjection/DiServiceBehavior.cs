using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using KLib.DependencyInjection;

namespace KLib.WcfExtension.DependencyInjection
{
    public sealed class DiServiceBehavior : Attribute, IServiceBehavior
    {
        private readonly IResolverFactory _resolverFactory;

        public DiServiceBehavior(Type resolverFactoryType)
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

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
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
            if (!typeof (IResolverFactory).IsAssignableFrom(factoryType))
                throw new ArgumentException("Expecting a Resolver factory type which implements IResolverFactory");
        }
    }
}
