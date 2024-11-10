using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Backend.Attributes
{
    public static class AttributeFinder
    {
        public static IEnumerable<Type> FindClassesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            // Get all types in the current assembly
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttributes(typeof(TAttribute), false).Any());
        }
    }
}