using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using KLib.DataAccess;
using KLib.DependencyInjection.Ninject;
using Ninject.Modules;

namespace TestApp
{
    class TestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStudentRepository>().To<StudentRepository>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new DiRepositoryFactory(
                new NinjectResolver(new TestModule())
                );
            ContextManager.RepositoryFactories.Add(factory);

            using (var cm = new ContextManager())
            {
                var studentRepository = cm.Repository<IStudentRepository>();
                Debug.Assert(cm.Context<TestDBEntities>() == studentRepository.Context);

            }
        }
    }
}
