using KLib.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UnitTest.KLib.DependencyInject.TestClasses;

namespace UnitTest.KLib.DependencyInject
{
    
    
    /// <summary>
    ///This is a test class for TypeHelpersTest and is intended
    ///to contain all TypeHelpersTest Unit Tests
    ///</summary>
    [TestClass]
    public class TypeHelpersTest
    {
        
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }        

        /// <summary>
        ///A test for GetBestMatch
        ///</summary>
        [TestMethod]
        public void GetBestMatchTest()
        {
            var input1 = new object[0];
            var input3 = new object[] {"123", 1.4};
            var input4 = new object[] { new TypeHelpersTestClass(), "Value" };
            var input5 = new object[] {100, 10};
            var input6 = new object[] {new[] {"Value", "Test"}};

            var constructors = typeof (TypeHelpersTestClass).GetConstructors();

            var bestMatch1 = TypeHelpers.GetBestMatch(constructors, input1);
            Assert.IsNotNull(bestMatch1);
            Assert.AreEqual(0, bestMatch1.GetParameters().Length);

            var bestMatch2 = TypeHelpers.GetBestMatch(constructors, null);
            Assert.IsNotNull(bestMatch2);
            Assert.AreEqual(0, bestMatch2.GetParameters().Length);

            var bestMatch3 = TypeHelpers.GetBestMatch(constructors, input3);
            Assert.IsNotNull(bestMatch3);
            Assert.AreEqual(2, bestMatch3.GetParameters().Length);
            Assert.AreEqual("message", bestMatch3.GetParameters()[0].Name);
            Assert.AreEqual(typeof (string), bestMatch3.GetParameters()[0].ParameterType);
            Assert.AreEqual("value", bestMatch3.GetParameters()[1].Name);
            Assert.AreEqual(typeof(double), bestMatch3.GetParameters()[1].ParameterType);

            var bestMatch4 = TypeHelpers.GetBestMatch(constructors, input4);
            Assert.IsNotNull(bestMatch4);
            Assert.AreEqual(2, bestMatch4.GetParameters().Length);
            Assert.AreEqual("instance", bestMatch4.GetParameters()[0].Name);
            Assert.AreEqual(typeof(TypeHelpersTestClass), bestMatch4.GetParameters()[0].ParameterType);
            Assert.AreEqual("text", bestMatch4.GetParameters()[1].Name);
            Assert.AreEqual(typeof(string), bestMatch4.GetParameters()[1].ParameterType);

            var bestMatch5 = TypeHelpers.GetBestMatch(constructors, input5);
            Assert.IsNotNull(bestMatch5);
            Assert.AreEqual(2, bestMatch5.GetParameters().Length);
            Assert.AreEqual("text", bestMatch5.GetParameters()[0].Name);
            Assert.AreEqual(typeof(int), bestMatch5.GetParameters()[0].ParameterType);
            Assert.AreEqual("number", bestMatch5.GetParameters()[1].Name);
            Assert.AreEqual(typeof(int), bestMatch5.GetParameters()[1].ParameterType);

            var bestMatch6 = TypeHelpers.GetBestMatch(constructors, input6);
            Assert.IsNotNull(bestMatch6);
            Assert.AreEqual(1, bestMatch6.GetParameters().Length);
            Assert.AreEqual("texts", bestMatch6.GetParameters()[0].Name);
            Assert.AreEqual(typeof(string[]), bestMatch6.GetParameters()[0].ParameterType);
           
        }

        [TestMethod]
        public void TestGetBestMatchFail()
        {
            var inputs = new[]
                {
                    new object[] {10},
                    new object[] {"Test"},
                    new object[] {"Test", 10},
                    new object[] {"Test", new TypeHelpersTestClass()},
                    new object[] {"Test", 10, "Text"}
                };

            var constructors = typeof(TypeHelpersTestClass).GetConstructors();

            foreach (var constructor in inputs.Select(input => TypeHelpers.GetBestMatch(constructors, input)))
            {
                Assert.IsNull(constructor);
            }
        }
    }
}
