using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using KLib.DependencyInjection;

namespace KLib.WcfExtension
{
    public class DiInstanceContextInitializer : IInstanceContextInitializer
    {
        private readonly IResolver _resolver;

        public DiInstanceContextInitializer(IResolverFactory resolverFactory)
        {
            _resolver = resolverFactory.CreateResolver();
        }

        public void Initialize(InstanceContext instanceContext, Message message)
        {
            // Ensure handler's removal
            instanceContext.Opened -= InstanceContextOpendedHandler;

            // Add event handler for Opened event
            instanceContext.Opened += InstanceContextOpendedHandler;
        }

        /// <summary>
        /// Handler for injecting dependencies when Service instance is ready to be used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstanceContextOpendedHandler(object sender, EventArgs e)
        {
            if (!(sender is InstanceContext)) return;

            // Inject dependencies on service instance
            var context = sender as InstanceContext;
            var serviceInstance = context.GetServiceInstance();
            _resolver.ResolveDependencies(serviceInstance);
        }
    }
}
