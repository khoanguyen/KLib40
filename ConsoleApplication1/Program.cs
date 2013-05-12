using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TestDBEntities())
            {
                ObjectStateEntry entry;
                var student = new Student {Id = 4, ComplexProperty_StudentName = "sadasD"};
                if (!context.ObjectStateManager.TryGetObjectStateEntry(student, out entry))
                {
                    context.Students.Attach(student);
                    entry = context.ObjectStateManager.GetObjectStateEntry(student);
                }

            }
        }
    }
}
