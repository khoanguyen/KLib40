using System;

namespace KLib.WcfExtension
{
    internal static class TypeHelper
    {
        public static bool IsTypeOf<T>(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static T CreateInstance<T>(Type type)
        {
            return (T) Activator.CreateInstance(type);
        }
    }
}
