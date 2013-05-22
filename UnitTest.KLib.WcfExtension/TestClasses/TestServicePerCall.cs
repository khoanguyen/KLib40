using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace UnitTest.KLib.WcfExtension.TestClasses
{    
    class TestServicePerCall : ITestService
    {
        public IStringProvider StringProvider { get; set; }

        public string SayHello()
        {
            return StringProvider.GetGreeting();
        }
    }
}
