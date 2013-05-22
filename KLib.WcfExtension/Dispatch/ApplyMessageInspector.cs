using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace KLib.WcfExtension.Dispatch
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
            if (!InspectorIs<IClientMessageInspector>()) return;
            var inspector = CreateInspector<IClientMessageInspector>();
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if (!InspectorIs<IDispatchMessageInspector>()) return;
            var inspector = CreateInspector<IDispatchMessageInspector>();
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
            if (!InspectorIs<IDispatchMessageInspector>()) return;
            var inspector = CreateInspector<IDispatchMessageInspector>();
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                }
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
            if (!InspectorIs<IDispatchMessageInspector>()) return;
            var inspector = CreateInspector<IDispatchMessageInspector>();
            dispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                        ClientRuntime clientRuntime)
        {
            if (!InspectorIs<IClientMessageInspector>()) return;
            var inspector = CreateInspector<IClientMessageInspector>();
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                         BindingParameterCollection bindingParameters)
        {
        }

        #endregion

        #region Helpers

        private T CreateInspector<T>()
        {
            return (T) Activator.CreateInstance(_inspectorType);
        }

        private bool InspectorIs<T>()
        {
            return typeof (IDispatchMessageInspector).IsAssignableFrom(_inspectorType);
        }

        #endregion
    }
}
