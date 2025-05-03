using System;
using System.Linq;
using System.Reflection;
using Backend.Attributes;
using Backend.Components;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    [Action]
    public class DestroyByTypeAction : EasyAction
    {
        // Needs to stay "public" otherwise it won't work
        public string targetTypeName;
        private Type targetType;
        
        public DestroyByTypeAction()
        {
            actionName = "Destroy by Type";
            actionDescription = "Destroys the colliding object that matches the selected type.";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            var types = Assembly.GetAssembly(typeof(BaseComponent))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseComponent)) && !t.IsAbstract)
                .ToArray();
            var names = types.Select(t => t.Name).ToArray();
            
            // Dropdown
            int idx = Array.IndexOf(names, targetTypeName);
            idx = Mathf.Clamp(EditorGUILayout.Popup("Target Type", idx, names), 0, names.Length - 1);
            targetTypeName = names[idx];

            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            // Get type object by reflection
            var asm   = Assembly.GetAssembly(typeof(BaseComponent));
            targetType = asm
                .GetTypes()
                .FirstOrDefault(t => t.Name == targetTypeName);

            if (targetType == null)
            {
                Debug.LogError($"[DestroyByTypeAction] Can not find type: '{targetTypeName}'");
                return;
            }

            // If source object matches, destroy it
            if (source != null && source.GetType() == targetType)
            {
                GameObject.Destroy(source.gameObject);
            }

            // If other object matches, destroy it
            if (other != null && other.GetType() == targetType)
            {
                GameObject.Destroy(other.gameObject);
            }
        }
    }
}