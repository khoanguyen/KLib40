using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace KLib.WcfExtension.Configurations
{
    public class ApplyOperationInvokerExtensionElement : BehaviorExtensionElement
    {
        private const string InvokerTypeKey = "invokerType";

        [ConfigurationProperty(InvokerTypeKey)]
        public string InvokerType
        {
            get { return (string)this[InvokerTypeKey]; }
            set { this[InvokerTypeKey] = value; }
        }

        protected override object CreateBehavior()
        {
            var type = Type.GetType(InvokerType);
            return new ApplyOperationInvoker(type);
        }

        public override Type BehaviorType
        {
            get { return typeof(ApplyOperationInvoker); }
        }
    }
}
