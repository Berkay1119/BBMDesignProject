using System;
using System.Collections.Generic;
using Backend.Attributes;
using Backend.Components;

namespace Backend.Controllers
{
    public static class ComponentController
    {
        public static List<BaseComponent> FindComponents()
        {
            var components = new List<BaseComponent>();
            var listOfComponentTypes=AttributeFinder.FindClassesWithAttribute<ComponentAttribute>();
            
            foreach (var componentType in listOfComponentTypes)
            {
                var component = (BaseComponent)System.Activator.CreateInstance(componentType);
                components.Add(component);
            }
            
            return components;
        }
    }
}