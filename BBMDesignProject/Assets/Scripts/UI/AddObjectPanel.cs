using System.Collections.Generic;
using Backend.Components;
using Backend.Objects;
using UnityEditor;
using UnityEngine;

namespace UI {
    public class AddObjectWindow : EditorWindow
    {
        private string objectName = "";
        private Texture2D objectTexture;
        private bool isPlatform;
        private bool isPlatformCharacter;
        private bool isCollectible;
        private bool isObstacle;

        public static void ShowWindow()
        {
            GetWindow<AddObjectWindow>("Add New Object");
        }

        private void OnGUI()
        {
            // Object name input
            objectName = EditorGUILayout.TextField("Object Name:", objectName);

            // Object texture (png) input
            objectTexture = (Texture2D)EditorGUILayout.ObjectField("Object Texture (PNG):", objectTexture, typeof(Texture2D), false);

            GUILayout.Label("Components and Behaviours:", EditorStyles.boldLabel);
            
            DrawToggleWithDescription(ref isPlatform, "Platform", "This object acts as a platform.");
            DrawToggleWithDescription(ref isPlatformCharacter, "Platform Character", "Character that interacts with platforms.");
            DrawToggleWithDescription(ref isCollectible, "Collectible", "Collectible item that the player can collect.");
            DrawToggleWithDescription(ref isObstacle, "Obstacle", "Object that acts as an obstacle for the player.");
            
            
            // Add functionality
            if (GUILayout.Button("Add"))
            {
                Debug.Log($"Object Added: {objectName}");
                
                if (objectTexture != null)
                {
                    Debug.Log($"Texture assigned: {objectTexture.name}");
                }
                else
                {
                    Debug.Log("No texture assigned.");
                }

                // Debugging
                Debug.Log("Selected Components and Behaviours:");
                if (isPlatform) Debug.Log("- Platform");
                if (isPlatformCharacter) Debug.Log("- Platform Character");
                if (isCollectible) Debug.Log("- Collectible");
                if (isObstacle) Debug.Log("- Obstacle");
                Dictionary<string,bool> components = new Dictionary<string, bool>
                {
                    {"Platform", isPlatform},
                    {"Platform Character", isPlatformCharacter},
                    {"Collectible", isCollectible},
                    {"Obstacle", isObstacle}
                };
                var objectOnPanel = new ObjectOnPanel(objectTexture, objectName, components);
                PanelContentController.Instance.AddObjectOnPanel(objectOnPanel);
                
                // Auto-closing after adding
                Close();
            }
            
        }
        
        // Helper func to adjust toggles and descriptions
        private void DrawToggleWithDescription(ref bool toggleValue, string label, string description)
        {
            GUILayout.BeginHorizontal();
            toggleValue = EditorGUILayout.Toggle(toggleValue, GUILayout.Width(20)); // Toggle on the left
            GUILayout.Label($"{label}: {description}", GUILayout.ExpandWidth(true)); // Description on the right
            GUILayout.EndHorizontal();
        }
    }
}