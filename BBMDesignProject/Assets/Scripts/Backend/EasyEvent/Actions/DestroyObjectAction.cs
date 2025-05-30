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
    public class DestroyObjectAction : EasyAction
    {
        // Needs to stay "public" otherwise it won't work
        public string targetTypeName;
        public string targetTag = "Untagged";
        private Type targetType;
        
        public DestroyObjectAction()
        {
            actionName = "Destroy Object";
            actionDescription = "Destroys object that matches the selected type and tag.";
        }

        public override void DrawGUI()
        {
            GUILayout.Space(10);
            base.DrawGUI();
            GUILayout.Space(10);
            
            // Component Type Dropdown
            var types = Assembly.GetAssembly(typeof(BaseComponent))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseComponent)) && !t.IsAbstract)
                .ToArray();
            
            var names = types.Select(t => t.Name).ToArray();
            int idx = Array.IndexOf(names, targetTypeName);
            idx = Mathf.Clamp(EditorGUILayout.Popup("Target Type", idx, names), 0, names.Length - 1);
            targetTypeName = names[idx];
            
            // Tag Dropdown
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            int tagIdx = Array.IndexOf(tags, targetTag);
            tagIdx = Mathf.Clamp(EditorGUILayout.Popup("Target Tag", tagIdx, tags), 0, tags.Length - 1);
            targetTag = tags[tagIdx];
            
            // HelpBox shown if no type or tag is selected
            if (string.IsNullOrEmpty(targetTypeName) || string.IsNullOrEmpty(targetTag) || targetTag == "Untagged")
            {
                EditorGUILayout.HelpBox("Please select an object type and a tag.", MessageType.Info);
            }

            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            // Get type object by reflection
            var asm = Assembly.GetAssembly(typeof(BaseComponent));
            targetType = asm
                .GetTypes()
                .FirstOrDefault(t => t.Name == targetTypeName);

            if (targetType == null)
            {
                Debug.LogError($"[DestroyObjectAction] Can not find type: '{targetTypeName}'");
                return;
            }

            bool Matches(GameObject go, BaseComponent comp) =>
                comp != null &&
                comp.GetType() == targetType;
                //go.CompareTag(targetTag);

            // If source object matches, destroy it
            if (source != null && Matches(source.gameObject, source))
            {
                GameObject.Destroy(source.gameObject);
            }

            // If other object matches, destroy it
            if (other != null && Matches(other.gameObject, other))
            {
                GameObject.Destroy(other.gameObject);
            }
        }
    }
}