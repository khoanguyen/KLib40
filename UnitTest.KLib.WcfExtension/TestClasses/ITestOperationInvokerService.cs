using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    [ServiceContract]
    public interface ITestOperationInvokerService
    {
        [OperationContract]
        string SayHello();

        [OperationContract]
        string SayGoodBye();
    }
}
