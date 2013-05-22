using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class TestServicePerSession : ITestService
    {
        public IStringProvider StringProvider { get; set; }

        public string SayHello()
        {
            return StringProvider.GetGreeting();
        }
    }
}
