using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace KLib.WcfExtension
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface)]
    public sealed class ApplyMessageInspector : Attribute, IServiceBehavior, IEndpointBehavior, IContractBehavior
    {
        #region Members

        private readonly Type _inspectorType;

        #endregion

        public ApplyMessageInspector(Type messageInspectorType)
        {
            _inspectorType = messageInspectorType;
        }

        #region IEndpointBehavior

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (!TypeHelper.IsTypeOf<IClientMessageInspector>(_inspectorType)) return;
            var inspector = TypeHelper.CreateInstance<IClientMessageInspector>(_inspectorType);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (!TypeHelper.IsTypeOf<IDispatchMessageInspector>(_inspectorType)) return;
            var inspector = TypeHelper.CreateInstance<IDispatchMessageInspector>(_inspectorType);
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);              
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IServiceBehavior

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (!TypeHelper.IsTypeOf<IDispatchMessageInspector>(_inspectorType)) return;
            var inspector = TypeHelper.CreateInstance<IDispatchMessageInspector>(_inspectorType);
            foreach (var endpointDispatcher in serviceHostBase.ChannelDispatchers
                                                             .OfType<ChannelDispatcher>()
                                                             .SelectMany(cd => cd.Endpoints))
            {
                endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);                
            }            
        }

        #endregion

        #region IContractBehavior

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                          DispatchRuntime dispatchRuntime)
        {
            if (!TypeHelper.IsTypeOf<IDispatchMessageInspector>(_inspectorType)) return;
            var inspector = TypeHelper.CreateInstance<IDispatchMessageInspector>(_inspectorType);
            dispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                        ClientRuntime clientRuntime)
        {
            if (!TypeHelper.IsTypeOf<IClientMessageInspector>(_inspectorType)) return;
            var inspector = TypeHelper.CreateInstance<IClientMessageInspector>(_inspectorType);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                         BindingParameterCollection bindingParameters)
        {
        }

        #endregion

    }
}
