using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.WcfExtension;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    
    public class TestOperationInvokerService : ITestOperationInvokerService
    {
        [ApplyOperationInvoker(typeof(TestOperationInvoker))]
        public string SayHello()
        {
            return "Hello";
        }

        public string SayGoodBye()
        {
            return "Good Bye";
        }
    }
}
