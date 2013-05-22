using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    class EnglishStringProvider : IStringProvider
    {
        public string GetGreeting()
        {
            return "Hello !!";
        }
    }
}
