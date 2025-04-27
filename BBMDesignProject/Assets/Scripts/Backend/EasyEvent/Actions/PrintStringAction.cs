using Backend.Attributes;
using Backend.Components;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    [Action]
    public class PrintStringAction : EasyAction
    {
        public string text;
        
        public PrintStringAction()
        {
            actionName = "PrintString";
            actionDescription = "Prints text to the console.";
        }
        
        
        public override void DrawGUI()
        {
            base.DrawGUI();
            text = EditorGUILayout.TextField("Text to Print",text);
            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            Debug.Log($"[PrintString] {text}");
        }
    }
}