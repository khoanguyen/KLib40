using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KLib.DataAccess;

namespace TestApp
{
    public class StudentRepository : Repository<TestDBEntities, Student>, IStudentRepository
    {
        public Student CreateNewStudent()
        {
            throw new NotImplementedException();
        }
    }
}
