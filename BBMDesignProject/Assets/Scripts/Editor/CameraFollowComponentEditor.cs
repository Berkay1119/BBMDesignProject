using Backend.Components;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(CameraFollowComponent))]
    public class CameraFollowComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CameraFollowComponent current = (CameraFollowComponent)target;

            // Find all instances of CameraFollowComponent in the scene
            CameraFollowComponent[] allComponents = FindObjectsOfType<CameraFollowComponent>();

            if (allComponents.Length > 1)
            {
                EditorGUILayout.HelpBox(
                    "Only one CameraFollowComponent should exist in the scene!",
                    MessageType.Error
                );
            }

            // Draw the default inspector below the warning (if any)
            DrawDefaultInspector();
        }
    }
}
