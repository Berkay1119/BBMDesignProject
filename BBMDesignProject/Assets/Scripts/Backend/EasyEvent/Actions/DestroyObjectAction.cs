using Backend.Attributes;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace Backend.Actions
{
    [Action]
    public class DestroyObjectAction:EasyAction
    {
        public EasyObject EasyObject;

        public override void DrawGUI()
        {
            base.DrawGUI();
            EasyObject = EditorGUILayout.ObjectField( "Object to Destroy", EasyObject , typeof(EasyObject), true ) as EasyObject;
            GUILayout.EndVertical();
        }

        public override void Execute()
        {
            EasyObject.DestroyEasyObject();
        }
    }
}