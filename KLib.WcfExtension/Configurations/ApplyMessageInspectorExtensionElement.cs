using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using KLib.WcfExtension.Dispatch;

namespace KLib.WcfExtension.Configurations
{
    /// <summary>
    /// Extension element for configuring ApplyMessageInspector behavior in config file
    /// </summary>
    public class ApplyMessageInspectorExtensionElement : BehaviorExtensionElement
    {        
        private const string InspectorTypeKey = "inspectorType";

        [ConfigurationProperty(InspectorTypeKey)]
        public string InspectorType
        {
            get { return (string)this[InspectorTypeKey]; }
            set { this[InspectorTypeKey] = value; }
        }

        protected override object CreateBehavior()
        {
            var type = Type.GetType(InspectorType);
            return new ApplyMessageInspector(type);
        }

        public override Type BehaviorType
        {
            get { return typeof(ApplyMessageInspector); }
        }
    }
}
