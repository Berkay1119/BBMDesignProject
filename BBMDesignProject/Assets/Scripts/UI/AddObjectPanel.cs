using System.Collections.Generic;
using Backend.Components;
using Backend.Controllers;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace UI {
    public class AddObjectWindow : EditorWindow
    {
        private string objectName = "";
        private Texture2D objectTexture;
        private static List<BaseComponent> _availableComponents;
        
        // holds the users selection of components and behaviours
        private static List<bool> _componentsToggle = new List<bool>();

        public static void ShowWindow() {
            GetWindow<AddObjectWindow>("Add New EasyObject");
            _availableComponents = ComponentController.FindComponents();
            _componentsToggle.Clear();
            
            foreach (var baseComponent in _availableComponents) {
                _componentsToggle.Add(false);
            }
        }

        private void OnGUI() {
            objectName = EditorGUILayout.TextField("EasyObject Name:", objectName);

            objectTexture = (Texture2D)EditorGUILayout.ObjectField("EasyObject Texture (PNG):", objectTexture, typeof(Texture2D), false);

            GUILayout.Label("Components and Behaviours:", EditorStyles.boldLabel);
            
            for (var i = 0; i < _componentsToggle.Count; i++) {
                DrawToggleWithDescription(ref i, _availableComponents[i].Name, _availableComponents[i].Description);
                if ( _componentsToggle[i])
                {
                    _availableComponents[i].DrawGUI();
                }
            }

            // "add" button
            if (GUILayout.Button("Add")) {
                CreateNewObject();
            }
        }

        private void CreateNewObject() {
            
            if (string.IsNullOrWhiteSpace(objectName)) {
                Debug.LogWarning("EasyObject name cannot be empty!");
                return;
            }

            // create new gameObject
            GameObject newObject = new GameObject(objectName);
            var easyObject=newObject.AddComponent<EasyObject>();
            // add rect transform to the object
            //RectTransform rectTransform = newObject.AddComponent<RectTransform>();
           
            // default size
            //rectTransform.sizeDelta = new Vector2(3, 3); 

            // add sprite renderer to the object
            
            
            /*if (objectTexture != null) {
                SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
                Sprite sprite = Sprite.Create(objectTexture, 
                    new Rect(0, 0, objectTexture.width, objectTexture.height), 
                    new Vector2(0.5f, 0.5f)); // center pivot
                spriteRenderer.sprite = sprite;
            } else {
                Debug.Log("No texture provided, SpriteRenderer not added.");
            }*/

            if (objectTexture)
            {
                easyObject.AddSprite(objectTexture);
            }
            var selectedComponents = new Dictionary<string, bool>();
            for (var i = 0; i < _componentsToggle.Count; i++) {
                selectedComponents.Add(_availableComponents[i].Name, _componentsToggle[i]);
            }
            easyObject.CreateEasyComponents(selectedComponents);

            Debug.Log($"Created new object '{objectName}' with components:");
            foreach (var component in newObject.GetComponents<Component>()) {
                Debug.Log(component.GetType().Name);
            }
            
            // if object is a character or a platform, add boxCollider2D
            /*if (_componentsToggle[0] || _componentsToggle[1])
            {
                BoxCollider2D boxCollider2D = newObject.AddComponent<BoxCollider2D>();
                boxCollider2D.size = new Vector2(1, 1);
            }*/

            // select the new object in the hierarchy
            Selection.activeGameObject = newObject;

            // auto-closing after adding
            Close();
        }

        // draws toggles and descriptions
        private void DrawToggleWithDescription(ref int toggleIndex, string label, string description) {
            GUILayout.BeginHorizontal();
            _componentsToggle[toggleIndex] = EditorGUILayout.Toggle(_componentsToggle[toggleIndex], GUILayout.Width(20)); 
            GUILayout.Label($"{label}: {description}", GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
        }
    }
}
