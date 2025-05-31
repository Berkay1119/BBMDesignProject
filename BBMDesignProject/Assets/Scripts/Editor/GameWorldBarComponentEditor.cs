using Backend.Components;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameWorldBarComponent))]
    public class GameWorldBarComponentEditor:UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the default inspector
            DrawDefaultInspector();
            
            if (GUILayout.Button("Update Bar Sprite"))
            {
                GameWorldBarComponent component = (GameWorldBarComponent)target;
                component.UpdateSprite();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}