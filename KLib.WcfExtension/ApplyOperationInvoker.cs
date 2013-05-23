using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace KLib.WcfExtension
{
    public class ApplyOperationInvoker : Attribute, IServiceBehavior, IOperationBehavior
    {
        private readonly Type _invokerType;

        public ApplyOperationInvoker(Type invokerType)
        {
            if (invokerType == null) throw new ArgumentNullException("invokerType");
            if (!TypeHelper.IsTypeOf<IOperationInvoker>(invokerType))
                throw new ArgumentException("invokerType should implement IOperationInvoker interface");

            _invokerType = invokerType;
        }

        #region IServiceBehavior
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var operation in serviceHostBase.ChannelDispatchers
                                                             .OfType<ChannelDispatcher>()
                                                             .SelectMany(cd => cd.Endpoints)
                                                             .SelectMany(ep => ep.DispatchRuntime.Operations))
            {
                operation.Invoker = TypeHelper.CreateInstance<IOperationInvoker>(_invokerType);
            }
        } 
        #endregion

        #region IOperationBehavior
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {            
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = TypeHelper.CreateInstance<IOperationInvoker>(_invokerType);
        }

        public void Validate(OperationDescription operationDescription)
        {
        } 
        #endregion
    }
}
