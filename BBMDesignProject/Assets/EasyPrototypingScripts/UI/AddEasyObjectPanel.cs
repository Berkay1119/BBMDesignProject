using System.Collections.Generic;
using Backend.Components;
using Backend.Controllers;
using Backend.CustomVariableFeature;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace UI {
    public class AddEasyObjectWindow : EditorWindow
    {
        private string objectName = "";
        private Texture2D objectTexture;
        private static List<BaseComponent> _availableComponents;
        // holds the users selection of components and behaviours
        private static List<bool> _componentsToggle = new List<bool>();
        private bool cameraFollowObject = false;
        private List<CustomVariable> _customVariables = new List<CustomVariable>();
        private string newVariableName = "";
        private VariableType newVariableType = VariableType.String;
        private string newVariableValue = "";
        private Vector2 scrollPosition = Vector2.zero;
        
        public static void ShowWindow() {
            var window = GetWindow<AddEasyObjectWindow>("Add New EasyObject");
            window.minSize = new Vector2(Screen.currentResolution.width * 0.4f, Screen.currentResolution.height * 0.3f);
            _availableComponents = ComponentController.FindComponents();
            _componentsToggle.Clear();
            
            foreach (var baseComponent in _availableComponents) {
                _componentsToggle.Add(false);
            }
        }

        private void OnGUI() {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            var halfWidth = position.width * 0.5f;
            GUILayout.Space(10); 
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("EasyObject Name:", GUILayout.Width(200));
            objectName = GUILayout.TextField(objectName, GUILayout.Width(halfWidth));
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10); 

            GUILayout.BeginHorizontal();
            GUILayout.Label("EasyObject Texture (PNG):", GUILayout.Width(200));
            objectTexture = (Texture2D)EditorGUILayout.ObjectField(
                objectTexture,
                typeof(Texture2D),
                false,
                GUILayout.Width(halfWidth)
            );
            GUILayout.EndHorizontal();
            
            GUILayout.Space(30);
            
            GUILayout.Label("Select Object Behaviours:", EditorStyles.boldLabel);
            GUILayout.Space(5);
            
            for (var i = 0; i < _componentsToggle.Count; i++) {
                DrawToggleWithDescription(ref i, _availableComponents[i].Name, _availableComponents[i].Description);
            }
            
            GUILayout.Space(30); 
            
            GUILayout.Label("Custom Variables", EditorStyles.boldLabel);
            
            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal(); 
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(80)); 
            newVariableName = EditorGUILayout.TextField(newVariableName, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", GUILayout.Width(80)); 
            newVariableType = (VariableType)EditorGUILayout.EnumPopup(newVariableType, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Value", GUILayout.Width(80)); 
            newVariableValue = EditorGUILayout.TextField(newVariableValue, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(30))) {
                if (!string.IsNullOrWhiteSpace(newVariableName)) {
                    _customVariables.Add(new CustomVariable(newVariableName, newVariableType, newVariableValue));
                    newVariableName = "";
                    newVariableValue = "";
                }
            }

            GUILayout.EndHorizontal(); 

            // Display custom variables list
            for (int i = 0; i < _customVariables.Count; i++) {
                GUILayout.BeginHorizontal();
                
                if (GUILayout.Button("-", GUILayout.Width(30))) {
                    _customVariables.RemoveAt(i);
                }
                GUILayout.Label($"{_customVariables[i].Name} ({_customVariables[i].Type}): {_customVariables[i].Value}");
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.Space(50); 
            
            // Add new object button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); 
            if (GUILayout.Button("Create Easy Object!", GUILayout.Width(halfWidth/2))) {
                CreateNewObject();
            }
            GUILayout.FlexibleSpace(); 
            GUILayout.EndHorizontal();
            
            EditorGUILayout.EndScrollView();
        }

        private void CreateNewObject() {
            
            if (string.IsNullOrWhiteSpace(objectName)) {
                Debug.LogWarning("EasyObject name can not be empty!");
                return;
            }

            // Create a new gameObject
            GameObject newObject = new GameObject(objectName);
            var easyObject=newObject.AddComponent<EasyObject>();

            if (objectTexture) {
                easyObject.AddSprite(objectTexture);
            } else {
                Debug.Log("No texture provided, SpriteRenderer not added.");
            }
            
            // Behaviours that user selected for the object
            var selectedComponents = new Dictionary<string, bool>();
            for (var i = 0; i < _componentsToggle.Count; i++) {
                selectedComponents.Add(_availableComponents[i].Name, _componentsToggle[i]);
            }
            
            easyObject.CreateEasyComponents(selectedComponents);
            
            // Apply custom variables
            foreach (var customVar in _customVariables) {
                easyObject.AddCustomVariable(customVar.Name, customVar.Type, customVar.Value.ToString());
            }

            Debug.Log("____________________________________________________");
            Debug.Log($"Created new object '{objectName}' with components:");
            foreach (var component in newObject.GetComponents<Component>()) {
                Debug.Log(component.GetType().Name);
            }
            Debug.Log("____________________________________________________");
            
            if (selectedComponents.ContainsKey("Character") && selectedComponents["Character"])
            {
                var characterComponent = newObject.GetComponent<AvatarComponent>();
                if (characterComponent != null && cameraFollowObject)
                {
                    var camGO = Camera.main?.gameObject;
                    if (camGO != null)
                    {
                        // // Check if there is already a CameraFollowComponent 
                        // var existingFollow = camGO.GetComponent<CameraFollowComponent>();
                        // if (existingFollow != null)
                        // {
                        //     // Update the target
                        //     var oldTarget = existingFollow.target;
                        //     existingFollow.target = newObject.transform;
                        //     Debug.Log($"Camera following target changed from '{oldTarget?.name}' to '{newObject.name}'.");
                        // } 
                        // else
                        // {
                        //     // Add a new CameraFollowComponent
                        //     var cameraFollow = camGO.AddComponent<CameraFollowComponent>();
                        //     cameraFollow.target = newObject.transform;
                        //     Debug.Log($"CameraFollowComponent added with target '{newObject.name}'");
                        // }
                    }
                }
            }
            
            // if (selectedComponents.ContainsKey("Platform") && selectedComponents["Platform"]){
            //     var platformComponent = newObject.GetComponent<SolidComponent>();
            //     if (platformComponent != null && isMovingPanel) {
            //         platformComponent.IsMoving = true; 
            //     }
            // }

            // Auto highlight the new object in the hierarchy
            Selection.activeGameObject = newObject;

            // Auto-closing after adding
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
