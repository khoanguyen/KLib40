using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using KLib.WcfExtension.Dispatch;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    [ServiceContract]
    [ApplyMessageInspector(typeof(TestMessageInspector))]
    public interface ITestService2
    {
        [OperationContract]
        string SayHello(string name);

        [OperationContract]
        string SayGoodBye(string name);
    }
}
