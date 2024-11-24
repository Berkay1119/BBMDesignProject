using System;
using Backend.Attributes;

namespace Backend.Components
{
    public class ComponentFactory
    {
        public static Type CreateComponent(string componentKey)
        {
            var classes=AttributeFinder.FindClassesWithAttribute<ComponentAttribute>();
            foreach (var componentType in classes)
            {
                var component = (BaseComponent)System.Activator.CreateInstance(componentType);
                if (component.Name == componentKey)
                {
                    return componentType;
                }
            }
            return null;
        }
    }
}