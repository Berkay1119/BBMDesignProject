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
            actionName = "Print String";
            actionDescription = "Prints text to the console";
        }
        
        
        public override void DrawGUI()
        {
            base.DrawGUI();
            text = EditorGUILayout.TextField("Text to Print", text);
            if (string.IsNullOrEmpty(text))
            {
                EditorGUILayout.HelpBox("Enter the message you want to display in the console.", MessageType.Info);
            }
            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            Debug.Log($"[PrintString Action] {text}");
        }
    }
}