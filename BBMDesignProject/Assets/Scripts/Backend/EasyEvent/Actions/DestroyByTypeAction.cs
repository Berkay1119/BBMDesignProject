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
        public string targetTypeName;
        private Type targetType;
        
        public DestroyByTypeAction()
        {
            actionName = "DestroyByType";
            actionDescription = "Çarpışan objelerden, seçilen türe sahip olana Destroy uygular";
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
            // Reflection ile tip objesini al
            var asm = Assembly.GetAssembly(typeof(BaseComponent));
            targetType = asm.GetType(targetTypeName);

            // Eğer kaynak objesi eşleşiyorsa yok et
            if (source != null && source.GetType() == targetType)
                GameObject.Destroy(source.gameObject);

            // Eğer diğer objesi eşleşiyorsa yok et
            if (other != null && other.GetType() == targetType)
                GameObject.Destroy(other.gameObject);
        }
    }
}