using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using KLib.WcfExtension.Dispatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.KLib.WcfExtension.TestClasses;

namespace UnitTest.KLib.WcfExtension
{
    [TestClass]
    public class ApplyMessageInspectorTest
    {
        private static TestServiceHost _host;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _host = new TestServiceHost();
            _host.Init();
        }

        [TestMethod]
        public void TestWithServiceBehavior()
        {
            _host.ServiceBehaviorHost.Open();
            TestMessageInspector.Prefix = "Service Behavior";
            var client = GetServiceClient<ITestService>(_host.ServiceBehaviorUri);
            client.SayHello();
            _host.ServiceBehaviorHost.BeginClose(null, null);

            Assert.AreEqual("Service Behavior After Receive Request", TestMessageInspector.AfterReceiveRequestMessage);
            Assert.AreEqual("Service Behavior Before Send Reply", TestMessageInspector.BeforeSendReplyMessage);
        }

        [TestMethod]
        public void TestWithContractBehavior()
        {
            _host.ContractBehaviorHost.Open();
            TestMessageInspector.Prefix = "Contract Behavior";
            var client = GetServiceClient<ITestService2>(_host.OperationBehaviorUri);
            client.SayHello("Khoa");

            Assert.AreEqual("Contract Behavior After Receive Request", TestMessageInspector.AfterReceiveRequestMessage);
            Assert.AreEqual("Contract Behavior Before Send Reply", TestMessageInspector.BeforeSendReplyMessage);

            Assert.AreEqual("Contract Behavior After Receive Reply", TestMessageInspector.AfterReceiveReplyMessage);
            Assert.AreEqual("Contract Behavior Before Send Request", TestMessageInspector.BeforeSendRequestMessage);

            _host.ContractBehaviorHost.BeginClose(null, null);

        }

        [TestMethod]
        public void TestWithEndpointBehavior()
        {
            _host.EndpointBeaviorHost.Open();
            TestMessageInspector.Prefix = "Endpoint Behavior";
            var client = GetServiceClient<ITestService>(_host.EndpointBehaviorUri);
            client.SayHello();

            Assert.AreEqual("Endpoint Behavior After Receive Request", TestMessageInspector.AfterReceiveRequestMessage);
            Assert.AreEqual("Endpoint Behavior Before Send Reply", TestMessageInspector.BeforeSendReplyMessage);

            TestMessageInspector.Reset();

            client = GetServiceClient<ITestService>(_host.EndpointBehaviorUri2);
            client.SayHello();

            Assert.AreEqual(string.Empty, TestMessageInspector.AfterReceiveRequestMessage);
            Assert.AreEqual(string.Empty, TestMessageInspector.BeforeSendReplyMessage);

            TestMessageInspector.Reset();
            client = GetServiceClientWithEndpointInspector<ITestService>(_host.EndpointBehaviorUri2);
            client.SayHello();

            Assert.AreEqual("Endpoint Behavior After Receive Reply", TestMessageInspector.AfterReceiveReplyMessage);
            Assert.AreEqual("Endpoint Behavior Before Send Request", TestMessageInspector.BeforeSendRequestMessage);

            _host.EndpointBeaviorHost.BeginClose(null, null);

        }

        private static T GetServiceClient<T>(Uri serviceUri)
        {
            var factory = new ChannelFactory<T>(new NetNamedPipeBinding(), new EndpointAddress(serviceUri));
            return factory.CreateChannel();
        }

        private static T GetServiceClientWithEndpointInspector<T>(Uri serviceUri)
        {
            var factory = new ChannelFactory<T>(new NetNamedPipeBinding(), new EndpointAddress(serviceUri));
            factory.Endpoint.Behaviors.Add(new ApplyMessageInspector(typeof (TestMessageInspector)));
            return factory.CreateChannel();
        }
    }
}
