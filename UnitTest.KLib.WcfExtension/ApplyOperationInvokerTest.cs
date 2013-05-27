using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.KLib.WcfExtension.TestClasses;

namespace UnitTest.KLib.WcfExtension
{
    [TestClass]
    public class ApplyOperationInvokerTest
    {
        private static TestServiceHost _host;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _host = new TestServiceHost();
            _host.Init();
        }

        [TestMethod]
        public void TestOperationInvokerAsOpeartionBehavior()
        {
            _host.OperationInvokerHost.Open();
            var client = GetServiceClient<ITestOperationInvokerService>(_host.OperationInvokerUri);
            client.SayHello();
            
            Assert.AreEqual("Test Operation Invoker Invoked", TestOperationInvoker.Message);

            _host.OperationInvokerHost.BeginClose(null, null);
        }

        [TestMethod]
        public void TestOperationInvokerAsServiceBehavior()
        {
            _host.OperationInvokerHost2.Open();
            var client = GetServiceClient<ITestOperationInvokerService>(_host.OperationInvokerUri2);
            client.SayHello();

            Assert.AreEqual("Test Operation Invoker Invoked", TestOperationInvoker.Message);

            TestOperationInvoker.Reset();
            Assert.AreEqual(string.Empty, TestOperationInvoker.Message);

            client.SayGoodBye();

            Assert.AreEqual("Test Operation Invoker Invoked", TestOperationInvoker.Message);

            _host.OperationInvokerHost.BeginClose(null, null);
        }

        private static T GetServiceClient<T>(Uri serviceUri)
        {
            var factory = new ChannelFactory<T>(new NetNamedPipeBinding(), new EndpointAddress(serviceUri));
            return factory.CreateChannel();
        }
    }
}
