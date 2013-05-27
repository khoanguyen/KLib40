using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.WcfExtension;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    [ApplyOperationInvoker(typeof(TestOperationInvoker))]
    class TestOperationInvokerServiceWithServiceBehavior : ITestOperationInvokerService
    {
        public string SayHello()
        {
            return "Hello";
        }

        public string SayGoodBye()
        {
            return "GoodBye";
        }
    }
}
