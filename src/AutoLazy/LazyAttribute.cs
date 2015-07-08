using System;
using System.Linq;

namespace AutoLazy
{
    /// <summary>
    /// Marks the specified method for lazy loading using the double check locking pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class LazyAttribute : Attribute
    {
    }
}
