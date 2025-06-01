using System;
using System.Linq;
using Backend.Components;
using Backend.CustomVariableFeature;
using Backend.Object;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(GameWorldBarComponent))]
    public class GameWorldBarComponentEditor:UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GameWorldBarComponent component = (GameWorldBarComponent)target;
            EditorGUILayout.LabelField("Game World Bar Component", EditorStyles.boldLabel);
            
            component.backgroundColor = EditorGUILayout.ColorField("Background Color", component.backgroundColor);
            component.fillColor = EditorGUILayout.ColorField("Fill Color", component.fillColor);
            
            component.barSprite = (Sprite)EditorGUILayout.ObjectField("Bar Sprite", component.barSprite, typeof(Sprite), false);
            component._fillImage = (Image)EditorGUILayout.ObjectField("Fill Image", component._fillImage, typeof(Image), false);
            component._valueBarUI = (GameObject)EditorGUILayout.ObjectField("Value Bar UI", component._valueBarUI, typeof(GameObject), true);
            component._backgroundImage = (Image)EditorGUILayout.ObjectField("Background Image", component._backgroundImage, typeof(Image), false);
            
            component.currentValueReferenceObject = (EasyObject)EditorGUILayout.ObjectField("Current Value Reference Object", 
                component.currentValueReferenceObject, typeof(EasyObject), true);

            if (component.currentValueReferenceObject != null)
            {
                var refVars = component.currentValueReferenceObject.GetComponents<SerializableCustomVariable>()
                    .Where(v => v.Type is VariableType.Integer or VariableType.Float)
                    .ToList();

                if (refVars.Count > 0)
                {
                    var refVarNames = refVars.Select(v => v.Name).ToArray();

                    if (!string.IsNullOrEmpty(component.currentValueNameString))
                    {
                        component.currentValueReferenceVariableIndex =
                            Array.IndexOf(refVarNames, component.currentValueNameString);
                    }
                    else
                    {
                        component.currentValueReferenceVariableIndex = 0;
                    }

                    if (component.currentValueReferenceVariableIndex < 0) component.currentValueReferenceVariableIndex = 0;

                    int newRefIndex = EditorGUILayout.Popup("Reference Variable", component.currentValueReferenceVariableIndex, refVarNames);

                    component.currentValueReferenceVariableIndex = newRefIndex;
                    component.currentValueNameString = refVarNames[component.currentValueReferenceVariableIndex];
                    component.currentValue  = refVars[component.currentValueReferenceVariableIndex];
                }
            }
            
            component.maxValueReferenceObject = (EasyObject)EditorGUILayout.ObjectField("Max Value Reference Object", 
                component.maxValueReferenceObject, typeof(EasyObject), true);

            if (component.maxValueReferenceObject != null)
            {
                var refVars = component.maxValueReferenceObject.GetComponents<SerializableCustomVariable>()
                    .Where(v => v.Type is VariableType.Integer or VariableType.Float)
                    .ToList();

                if (refVars.Count > 0)
                {
                    var refVarNames = refVars.Select(v => v.Name).ToArray();

                    if (!string.IsNullOrEmpty(component.maxValueNameString))
                    {
                        component.maxValueReferenceVariableIndex =
                            Array.IndexOf(refVarNames, component.maxValueNameString);
                    }
                    else
                    {
                        component.maxValueReferenceVariableIndex = 0;
                    }

                    if (component.maxValueReferenceVariableIndex < 0) component.maxValueReferenceVariableIndex = 0;

                    int newRefIndex = EditorGUILayout.Popup("Reference Variable", component.maxValueReferenceVariableIndex, refVarNames);

                    component.maxValueReferenceVariableIndex = newRefIndex;
                    component.maxValueNameString = refVarNames[component.maxValueReferenceVariableIndex];
                    component.maxValue  = refVars[component.maxValueReferenceVariableIndex];
                }
            }

            if (GUILayout.Button("Update Bar Sprite"))
            {
                component.UpdateSprite();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}