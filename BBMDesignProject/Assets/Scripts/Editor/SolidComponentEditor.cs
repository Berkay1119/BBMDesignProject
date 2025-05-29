using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Backend.Components.SolidComponent))]
    public class SolidComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty useGravityProp = serializedObject.FindProperty("useGravity");
            SerializedProperty gravityScaleProp = serializedObject.FindProperty("gravityScale");

            EditorGUILayout.PropertyField(useGravityProp);

            if (useGravityProp.boolValue)
            {
                EditorGUILayout.PropertyField(gravityScaleProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}