using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using KLib.DependencyInjection.Autofac;

namespace UnitTest.KLib.WcfExtension.TestClasses
{
    class TestResolverFactory : AutofacResolverFactory
    {
        public TestResolverFactory()
            : base(BuildAction)
        {
        }

        private static void BuildAction(ContainerBuilder builder)
        {
            builder.RegisterType<EnglishStringProvider>().As<IStringProvider>();
        }
    }
}
