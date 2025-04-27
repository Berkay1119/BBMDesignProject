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
        public string firstTypeName;
        public string secondTypeName;
        private Type firstType, secondType;
        
        public CollidingCondition()
        {
            conditionName = "CollidingCondition";
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
            
            // type 1
            int idx1 = Array.IndexOf(names, firstTypeName);
            idx1 = Mathf.Clamp(EditorGUILayout.Popup("Tip A", idx1, names), 0, names.Length - 1);
            firstTypeName = names[idx1];
            
            // type 2
            int idx2 = Array.IndexOf(names, secondTypeName);
            idx2 = Mathf.Clamp(EditorGUILayout.Popup("Tip B", idx2, names), 0, names.Length - 1);
            secondTypeName = names[idx2];
            
            GUILayout.EndVertical();
        }

        protected override void Subscribe()
        {
            if (string.IsNullOrEmpty(firstTypeName) || string.IsNullOrEmpty(secondTypeName))
            {
                Debug.LogWarning($"[{nameof(CollidingCondition)}] firstTypeName veya secondTypeName tanımsız. Abonelik atlandı.");
                return;
            }
            
            var asm = Assembly.GetAssembly(typeof(BaseComponent));
            firstType  = asm.GetType(firstTypeName);
            secondType = asm.GetType(secondTypeName);
            
            if (firstType == null || secondType == null)
            { 
                Debug.LogError($"[{nameof(CollidingCondition)}] Tip bulunamadı: {firstTypeName}, {secondTypeName}"); 
                return;
            }
            
            EventBus.OnCollision2D += OnAnyCollision;
        } 
        
        protected override void Unsubscribe()
        {
            EventBus.OnCollision2D -= OnAnyCollision;
        }

        private void OnAnyCollision(BaseComponent a, BaseComponent b)
        {
            if ((a.GetType() == firstType && b.GetType() == secondType) ||
                (a.GetType() == secondType && b.GetType() == firstType))
            {
                foreach (var action in relatedEvent.Actions)
                    action.Execute(a, b);
            }
        }
    }
}