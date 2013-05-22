﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using KLib.WcfExtension.DependencyInjection;

namespace KLib.WcfExtension.Configurations
{
    /// <summary>
    /// Extension Element for configuring WCF DI
    /// </summary>
    public class DiServiceBehaviorExtensionElement : BehaviorExtensionElement
    {
        private const string ResolverFactoryTypeKey = "resolverFactoryType";

        [ConfigurationProperty(ResolverFactoryTypeKey)]
        public string ResolverFactoryType
        {
            get { return (string)this[ResolverFactoryTypeKey]; }
            set { this[ResolverFactoryTypeKey] = value; }
        }

        public override Type BehaviorType
        {
            get { return typeof(DiServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            // Get Factory type and create service behavior
            var resolverFactoryType = Type.GetType(ResolverFactoryType);
            return new DiServiceBehavior(resolverFactoryType);
        }
    }
}
