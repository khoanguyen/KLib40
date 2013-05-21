using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DataAccess
{
    /// <summary>
    /// Exception fired by ContextManager when there is any error during Saving changes on DbContext/ObjectContext
    /// </summary>
    public class SaveChangesException : Exception
    {

        public SaveChangesException(Exception inner)
            : base("Error when saving changes", inner)
        {
        }
    }
}
