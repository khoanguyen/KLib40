using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using KLib.WcfExtension;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    public class TestOperationInvoker : OperationInvokerBase
    {
        private readonly IOperationInvoker _original;

        public static String Message;

        public static void Reset()
        {
            Message = string.Empty;
        }

        public TestOperationInvoker(DispatchOperation dispatchOperation)
            : base(dispatchOperation)
        {
            _original = DispatchOperation.Invoker;
            IsSynchronous = _original.IsSynchronous;
        }

        public override object[] AllocateInputs()
        {
            return _original.AllocateInputs();
        }

        public override object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            Message = "Test Operation Invoker Invoked";
            return _original.Invoke(instance, inputs, out outputs);
        }

        public override IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _original.InvokeBegin(instance, inputs, callback, state);
        }

        public override object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return _original.InvokeEnd(instance, out outputs, result);
        }

        public override bool IsSynchronous { get; protected set; }
    }
}
