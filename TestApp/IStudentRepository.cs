using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.DataAccess;

namespace TestApp
{
    public interface IStudentRepository : IRepository<TestDBEntities, Student>
    {
        Student CreateNewStudent();
    }
}
