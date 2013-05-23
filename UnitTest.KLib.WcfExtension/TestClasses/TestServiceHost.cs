using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using KLib.WcfExtension;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    public class TestServiceHost
    {
        public Uri PerCallServiceUri { get; private set; }
        //public Uri PerSessionServiceUri { get; private set; }
        public Uri SingleServiceUri { get; private set; }
        public Uri ServiceBehaviorUri { get; private set; }
        public Uri OperationBehaviorUri { get; private set; }
        public Uri EndpointBehaviorUri { get; private set; }
        public Uri EndpointBehaviorUri2 { get; private set; }

        public ServiceHost PerCallHost { get; private set; }
        public ServiceHost SingleHost { get; private set; }
        public ServiceHost ServiceBehaviorHost { get; private set; }
        public ServiceHost ContractBehaviorHost { get; private set; }
        public ServiceHost EndpointBeaviorHost { get; private set; }

        public void Init()
        {
            PerCallServiceUri = new Uri("net.pipe://localhost/TestServicePerCall");
            //PerSessionServiceUri = new Uri("net.pipe://localhost/TestServicePerSession");
            SingleServiceUri = new Uri("net.pipe://localhost/TestServiceSingle");
            ServiceBehaviorUri = new Uri("net.pipe://localhost/TestServiceBehavior");
            OperationBehaviorUri = new Uri("net.pipe://localhost/TestContractBehavior");
            EndpointBehaviorUri = new Uri("net.pipe://localhost/TestEndpointBehavior");
            EndpointBehaviorUri2 = new Uri("net.pipe://localhost/TestEndpointBehavior2");
            //var binding = new NetNamedPipeBinding();

            PerCallHost = new ServiceHost(typeof (TestServicePerCall));
            PerCallHost.AddServiceEndpoint(typeof (ITestService), new NetNamedPipeBinding(), PerCallServiceUri);
            PerCallHost.Description.Behaviors.Add(new ApplyDependencyInjection(typeof (TestResolverFactory)));

            SingleHost = new ServiceHost(typeof (TestServiceSingle));
            SingleHost.AddServiceEndpoint(typeof (ITestService), new NetNamedPipeBinding(), SingleServiceUri);
            SingleHost.Description.Behaviors.Add(new ApplyDependencyInjection(typeof (TestResolverFactory)));

            ServiceBehaviorHost = new ServiceHost(typeof (TestServiceWithServiceBehavior));
            ServiceBehaviorHost.AddServiceEndpoint(typeof (ITestService), new NetNamedPipeBinding(), ServiceBehaviorUri);

            ContractBehaviorHost = new ServiceHost(typeof (TestServiceWithContractBehavior));
            ContractBehaviorHost.AddServiceEndpoint(typeof (ITestService2), new NetNamedPipeBinding(),
                                                    OperationBehaviorUri);

            EndpointBeaviorHost = new ServiceHost(typeof (TestServiceSingle));
            EndpointBeaviorHost.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), EndpointBehaviorUri);
            EndpointBeaviorHost.AddServiceEndpoint(typeof(ITestService), new NetNamedPipeBinding(), EndpointBehaviorUri2);
            EndpointBeaviorHost.Description.Behaviors.Add(new ApplyDependencyInjection(typeof(TestResolverFactory)));
            EndpointBeaviorHost.Description
                               .Endpoints
                               .Single(ep => ep.Address.Uri == EndpointBehaviorUri)
                               .Behaviors
                               .Add(new ApplyMessageInspector(typeof (TestMessageInspector)));
        }
    }
}
