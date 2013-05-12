using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KLib.DependencyInjection
{
    public interface IResolverFactory
    {
        IResolver CreateResolver();
    }
}
