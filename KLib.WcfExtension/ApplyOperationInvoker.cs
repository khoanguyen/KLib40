using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace KLib.WcfExtension
{
    public class ApplyOperationInvoker : Attribute, IOperationBehavior
    {
        private readonly Type _invokerType;

        public ApplyOperationInvoker(Type invokerType)
        {
            if (invokerType == null) throw new ArgumentNullException("invokerType");
            if (!TypeHelper.IsTypeOf<OperationInvokerBase>(invokerType))
                throw new ArgumentException("invokerType should inherit KLib.WcfExtension.OperationInvokerBase");
            _invokerType = invokerType;
        }        

        #region IOperationBehavior
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {            
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = TypeHelper.CreateInstance<IOperationInvoker>(_invokerType, dispatchOperation);
        }

        public void Validate(OperationDescription operationDescription)
        {
        } 
        #endregion
    }
}
