using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace KLib.WcfExtension
{
    public abstract class OperationInvokerBase : IOperationInvoker
    {
        public DispatchOperation DispatchOperation { get; private set; }

        public OperationInvokerBase(DispatchOperation dispatchOperation)
        {
            DispatchOperation = dispatchOperation;
        }

        public abstract object[] AllocateInputs();
        public abstract object Invoke(object instance, object[] inputs, out object[] outputs);
        public abstract IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state);
        public abstract object InvokeEnd(object instance, out object[] outputs, IAsyncResult result);
        public abstract bool IsSynchronous { get; protected set; }
    }
}
