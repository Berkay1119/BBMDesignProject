using System;
using System.Linq;
using System.Reflection;
using Backend.Attributes;
using Backend.Components;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Conditions
{
    [Condition]
    public class CollidingCondition : EasyCondition
    {
        // Needs to stay "public" otherwise it won't work
        public string firstTypeName;
        public string secondTypeName;
        private Type firstType, secondType;
        
        public CollidingCondition()
        {
            conditionName = "Colliding Condition";
            conditionDescription = "Check if two objects are colliding";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            
            var types = Assembly.GetAssembly(typeof(BaseComponent))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseComponent)) && !t.IsAbstract)
                .ToArray();
            var names = types.Select(t => t.Name).ToArray();
            
            // Type 1
            int idx1 = Array.IndexOf(names, firstTypeName);
            idx1 = Mathf.Clamp(EditorGUILayout.Popup("First Type", idx1, names), 0, names.Length - 1);
            firstTypeName = names[idx1];
            
            // Type 2
            int idx2 = Array.IndexOf(names, secondTypeName);
            idx2 = Mathf.Clamp(EditorGUILayout.Popup("Second Type", idx2, names), 0, names.Length - 1);
            secondTypeName = names[idx2];
            
            GUILayout.EndVertical();
        }

        protected override void Subscribe()
        {
            if (string.IsNullOrEmpty(firstTypeName) || string.IsNullOrEmpty(secondTypeName))
            {
                //Debug.LogWarning($"[{nameof(CollidingCondition)}] Type-1 or Type-2 is undefined. Subscription skipped.");
                return;
            }
            
            var asm = Assembly.GetAssembly(typeof(BaseComponent));
            firstType  = asm.GetType(firstTypeName);
            secondType = asm.GetType(secondTypeName);

            foreach (var type in asm.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseComponent))))
            {
                if (type.Name == firstTypeName)
                { 
                    firstType = type;
                }
                else if (type.Name == secondTypeName)
                {
                    secondType = type;
                }
            }
            
            EventBus.OnCollision2D += OnAnyCollision;
        } 
        
        protected override void Unsubscribe()
        {
            EventBus.OnCollision2D -= OnAnyCollision;
        }

        private void OnAnyCollision(BaseComponent source, BaseComponent other)
        {
            if (source.GetType() == firstType && other.GetType() == secondType)
            {
                foreach (var action in relatedEvent.Actions)
                {
                    action.Execute(source, other);
                }
                    
            }
        }
        
    }
}