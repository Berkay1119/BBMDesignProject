using System;
using System.Linq;
using System.Reflection;
using Backend.Attributes;
using Backend.Components;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Backend.EasyEvent.Conditions
{
    [Condition]
    public class CollidingCondition : EasyCondition
    {
        // Needs to stay "public" otherwise it won't work
        public string firstTypeName;
        public string secondTypeName;
        
        public string firstRequiredTag;
        public string secondRequiredTag;
        
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
            
            // Tag list
            var allTags = InternalEditorUtility.tags;
            
            // Type 1
            int idx1 = Array.IndexOf(names, firstTypeName);
            idx1 = Mathf.Clamp(EditorGUILayout.Popup("First Type", idx1, names), 0, names.Length - 1);
            firstTypeName = names[idx1];
            
            // Tag 1 index
            int t1 = Array.IndexOf(allTags, firstRequiredTag);
            firstRequiredTag = allTags[Mathf.Clamp(
                EditorGUILayout.Popup("First Required Tag", t1, allTags),
                0, allTags.Length - 1
            )];

            // Type 2
            int idx2 = Array.IndexOf(names, secondTypeName);
            idx2 = Mathf.Clamp(EditorGUILayout.Popup("Second Type", idx2, names), 0, names.Length - 1);
            secondTypeName = names[idx2];
            
            // Tag 2 index
            int t2 = Array.IndexOf(allTags, secondRequiredTag);
            secondRequiredTag = allTags[Mathf.Clamp(
                EditorGUILayout.Popup("Second Required Tag", t2, allTags),
                0, allTags.Length - 1
            )];
            
            // Show HelpBox only if tags are not properly selected
            if (string.IsNullOrEmpty(firstRequiredTag) || string.IsNullOrEmpty(secondRequiredTag) ||
                firstRequiredTag == "Untagged" || secondRequiredTag == "Untagged")
            {
                EditorGUILayout.HelpBox("Select two types of objects you want to detect a collision between and their corresponding tags.", MessageType.Info);
            }
            
            GUILayout.EndVertical();
        }

        protected override void Subscribe()
        {
            if (string.IsNullOrEmpty(firstTypeName) || string.IsNullOrEmpty(secondTypeName))
            {
                Debug.LogWarning($"[{nameof(CollidingCondition)}] Type-1 or Type-2 is undefined. Subscription skipped.");
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
            // Type check
            if (source.GetType() != firstType || other.GetType() != secondType)
            {
                return;
            }

            // Tag check
            if (!source.CompareTag(firstRequiredTag) || !other.CompareTag(secondRequiredTag))
            {
                return;
            }

            foreach (var action in relatedEvent.Actions)
            {
                action.Execute(source, other);
            }
                
        }
        
    }
}