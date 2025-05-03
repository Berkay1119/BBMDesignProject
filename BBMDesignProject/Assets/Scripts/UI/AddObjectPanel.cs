using System.Collections.Generic;
using Backend.Components;
using Backend.Components.SubComponents;
using Backend.Controllers;
using Backend.CustomVariableFeature;
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

        private bool cameraFollowCharacter = false;
        private bool isMovingPanel = false;
        
        private List<CustomVariable> _customVariables = new List<CustomVariable>();
        private string newVariableName = "";
        private VariableType newVariableType = VariableType.String;
        private string newVariableValue = "";
        
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
               
                if (_availableComponents[i].Name == "Character" && _componentsToggle[i])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20); 
                    bool cameraFollow = GUILayout.Toggle(cameraFollowCharacter, "Camera: Follow Character");
                    cameraFollowCharacter = cameraFollow;
                    GUILayout.EndHorizontal();
                }
                if (_availableComponents[i].Name == "Platform" && _componentsToggle[i]) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20); 
                    isMovingPanel = GUILayout.Toggle(isMovingPanel, "Moving Panel");
                    GUILayout.EndHorizontal();
                }

                
                if ( _componentsToggle[i])
                {
                    _availableComponents[i].DrawGUI();
                }
            }
            
            GUILayout.Label("Custom Variables", EditorStyles.boldLabel);
            
            // Input fields for new variable
            GUILayout.BeginHorizontal();
            newVariableName = EditorGUILayout.TextField("Variable Name", newVariableName, GUILayout.Width(300));
            newVariableType = (VariableType)EditorGUILayout.EnumPopup("Type", newVariableType, GUILayout.Width(300));
            newVariableValue = EditorGUILayout.TextField("Value", newVariableValue, GUILayout.Width(300));
            
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
                GUILayout.Label($"{_customVariables[i].Name} ({_customVariables[i].Type}): {_customVariables[i].Value}");
                if (GUILayout.Button("Remove", GUILayout.Width(70))) {
                    _customVariables.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }
            

            // "add" button
            if (GUILayout.Button("Add")) {
                CreateNewObject();
            }
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
                var characterComponent = newObject.GetComponent<CharacterComponent>();
                if (characterComponent != null && cameraFollowCharacter)
                {
                    var camGO = Camera.main?.gameObject;
                    if (camGO != null)
                    {
                        // Check if there is already a CameraFollowComponent 
                        var existingFollow = camGO.GetComponent<CameraFollowComponent>();
                        if (existingFollow != null)
                        {
                            // Update the target
                            var oldTarget = existingFollow.target;
                            existingFollow.target = newObject.transform;
                            Debug.Log($"Camera following target changed from '{oldTarget?.name}' to '{newObject.name}'.");
                        } 
                        else
                        {
                            // Add a new CameraFollowComponent
                            var cameraFollow = camGO.AddComponent<CameraFollowComponent>();
                            cameraFollow.target = newObject.transform;
                            Debug.Log($"CameraFollowComponent added with target '{newObject.name}'");
                        }
                    }
                }
            }
            
            if (selectedComponents.ContainsKey("Platform") && selectedComponents["Platform"]){
                var platformComponent = newObject.GetComponent<PlatformComponent>();
                if (platformComponent != null && isMovingPanel) {
                    platformComponent.IsMoving = true; 
                }
            }

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
