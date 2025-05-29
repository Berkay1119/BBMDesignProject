using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Backend.Components.CollectibleComponent))]
    public class CollectibleComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // SerializedProperty useGravityProp = serializedObject.FindProperty("useGravity");
            // SerializedProperty gravityScaleProp = serializedObject.FindProperty("gravityScale");
            //
            // EditorGUILayout.PropertyField(useGravityProp);
            //
            // if (useGravityProp.boolValue)
            // {
            //     EditorGUILayout.PropertyField(gravityScaleProp);
            // }
            SerializedProperty physicsProp= serializedObject.FindProperty("isPhysicsActive");

            EditorGUILayout.PropertyField(physicsProp, new GUIContent("Use Physics"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
