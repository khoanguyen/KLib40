using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace KLib.WcfExtension.Configurations
{
    /// <summary>
    /// Extension element for configuring ApplyMessageInspector behavior in config file
    /// </summary>
    public class ApplyMessageInspectorExtensionElement : BehaviorExtensionElement
    {        
        private const string InspectorTypeKey = "inspectorType";

        [ConfigurationProperty(InspectorTypeKey, IsRequired = true)]
        public string InspectorTypeString
        {
            get { return (string)this[InspectorTypeKey]; }
            set { this[InspectorTypeKey] = value; }
        }

        protected override object CreateBehavior()
        {
            var type = Type.GetType(InspectorTypeString);
            return new ApplyMessageInspector(type);
        }

        public override Type BehaviorType
        {
            get { return typeof(ApplyMessageInspector); }
        }
    }
}
