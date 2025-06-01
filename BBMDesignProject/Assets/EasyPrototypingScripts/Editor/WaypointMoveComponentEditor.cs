using Backend.Components;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(WaypointMoveComponent))]
    [CanEditMultipleObjects]
    public class WaypointMoveComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.HelpBox(
                "- Create at least 2 empty EasyObjects and add them to the list. The object will move between these waypoints.\n" +
                "- Adjust 'One Loop Duration' to control the full cycle time.", 
                MessageType.Info);

            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}