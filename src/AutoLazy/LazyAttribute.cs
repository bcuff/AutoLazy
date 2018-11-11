using System;

namespace AutoLazy
{
    /// <summary>
    /// Marks the specified method for lazy loading using the double check locking pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public class LazyAttribute : Attribute
    {
    }
}
