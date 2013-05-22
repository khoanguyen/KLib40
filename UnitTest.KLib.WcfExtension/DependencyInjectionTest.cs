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
    public class DependencyInjectionTest
    {

        private static TestServiceHost _host;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _host = new TestServiceHost();
            _host.Init();
        }

        [TestMethod]
        public void TestDependencyInjectionPerCall()
        {
            //_host.Init();
            _host.PerCallHost.Open();
            var client = GetServiceClient(_host.PerCallServiceUri);
            var result = client.SayHello();
            _host.PerCallHost.BeginClose(null, null);
            Assert.AreEqual("Hello !!", result);
        }

        [TestMethod]
        public void TestDependencyInjectionSingle()
        {
            //_host.Init();
            _host.SingleHost.Open();
            var client = GetServiceClient(_host.SingleServiceUri);
            var result = client.SayHello();
            _host.SingleHost.BeginClose(null, null);
            Assert.AreEqual("Hello !!", result);
        }

        private static ITestService GetServiceClient(Uri serviceUri)
        {
            var factory = new ChannelFactory<ITestService>(new NetNamedPipeBinding(), new EndpointAddress(serviceUri));
            return factory.CreateChannel();
        }
    }
}
