using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.WcfExtension;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    [ApplyMessageInspector(typeof(TestMessageInspector))]
    public class TestServiceWithServiceBehavior : ITestService
    {
        public string SayHello()
        {
            return "Hello";
        }
    }
}
