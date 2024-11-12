using System.Collections.Generic;
using Backend.Components;
using Backend.Controllers;
using Backend.Objects;
using UnityEditor;
using UnityEngine;

namespace UI {
    public class AddObjectWindow : EditorWindow
    {
        private string objectName = "";
        private Texture2D objectTexture;
        private static List<BaseComponent> _availableComponents;
        private static List<bool> _componentsToggle = new List<bool>();

        public static void ShowWindow()
        {
            GetWindow<AddObjectWindow>("Add New Object");
            _availableComponents = ComponentController.FindComponents();
            _componentsToggle.Clear();
            // Initialize toggles
            foreach (var baseComponent in _availableComponents)
            {
                _componentsToggle.Add(false);
            }
        }

        private void OnGUI()
        {
            // Object name input
            objectName = EditorGUILayout.TextField("Object Name:", objectName);

            // Object texture (png) input
            objectTexture = (Texture2D)EditorGUILayout.ObjectField("Object Texture (PNG):", objectTexture, typeof(Texture2D), false);

            GUILayout.Label("Components and Behaviours:", EditorStyles.boldLabel);
            
           
            
            for (int i = 0; i < _componentsToggle.Count; i++)
            {
                DrawToggleWithDescription(ref i, _availableComponents[i].Name, _availableComponents[i].Description);
            }


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
                
                var _components = new Dictionary<string, bool>();
                for (int i = 0; i < _componentsToggle.Count; i++)
                {
                    _components.Add(_availableComponents[i].Name, _componentsToggle[i]);
                }
                
                var objectOnPanel = new ObjectOnPanel(objectTexture, objectName, _components);
                PanelContentController.Instance.AddObjectOnPanel(objectOnPanel);
                
                // Auto-closing after adding
                Close();
            }
            
        }
        
        // Helper func to adjust toggles and descriptions
        private void DrawToggleWithDescription(ref int toggleIndex, string label, string description)
        {
            GUILayout.BeginHorizontal();
            _componentsToggle[toggleIndex] = EditorGUILayout.Toggle(_componentsToggle[toggleIndex], GUILayout.Width(20)); // Toggle on the left
            GUILayout.Label($"{label}: {description}", GUILayout.ExpandWidth(true)); // Description on the right
            GUILayout.EndHorizontal();
        }
    }
}