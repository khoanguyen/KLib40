using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.WcfExtension.Dispatch;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    public class TestServiceWithContractBehavior : ITestService2
    {
        //[ApplyMessageInspector(typeof(TestMessageInspector))]
        public string SayHello(string name)
        {
            return string.Format("Hello {0}!", name);
        }

        public string SayGoodBye(string name)
        {
            return string.Format("Hello {0}!", name);
        }
    }
}
