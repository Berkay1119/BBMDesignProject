using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Actions
{
    [Action]
    public class PrintStringAction:EasyAction
    {
        public string text;
        public override void DrawGUI()
        {
            base.DrawGUI();
            text = EditorGUILayout.TextField("Text to Print",text);
            GUILayout.EndVertical();
        }

        public override void Execute()
        {
            Debug.Log(text);
        }
    }
}