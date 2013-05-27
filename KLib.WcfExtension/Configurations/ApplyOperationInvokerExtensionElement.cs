using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace KLib.WcfExtension.Configurations
{
    public class ApplyOperationInvokerExtensionElement : BehaviorExtensionElement
    {
        private const string InvokerTypeKey = "invokerType";

        [ConfigurationProperty(InvokerTypeKey, IsRequired = true)]
        public string InvokerTypeString
        {
            get { return (string)this[InvokerTypeKey]; }
            set { this[InvokerTypeKey] = value; }
        }

        public override Type BehaviorType
        {
            get { return typeof (ApplyOperationInvoker); }
        }

        protected override object CreateBehavior()
        {
            var invokerType = Type.GetType(InvokerTypeString);
            return new ApplyOperationInvoker(invokerType);
        }
    }
}
