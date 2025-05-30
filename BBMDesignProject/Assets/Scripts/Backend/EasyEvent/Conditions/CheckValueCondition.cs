using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Attributes;
using Backend.Components;
using Backend.CustomVariableFeature;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Conditions
{
    [Condition]
    public class CheckValueCondition : EasyCondition
    {
        public EasyObject targetObject;
        public int selectedIndex = 0;
        public string selectedVariableName;
        public VariableType selectedVariableType;
        public string valueToCompare;
        
        public CheckType checkType = CheckType.Equals;
        public ValueInputMode valueInputMode = ValueInputMode.EnterValue;

        public EasyObject referenceObject;
        public int referenceVariableIndex = 0;
        public string referenceVariableName;
        public SerializableCustomVariable referenceVariable;

        public CheckValueCondition()
        {
            conditionName = "Check Value Condition";
            conditionDescription = "Checks if a selected variable on a object matches the desired value.";
        }

        public override void DrawGUI()
        {
            GUILayout.Space(10);
            base.DrawGUI();
            GUILayout.Space(10);
            
            targetObject = (EasyObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(EasyObject), true);

            if (targetObject == null)
            {
                EditorGUILayout.HelpBox("Please assign a target object.", MessageType.Info);
                GUILayout.EndVertical();
                return;
            }

            var vars = targetObject.GetComponents<SerializableCustomVariable>();
            if (vars.Length == 0)
            {
                EditorGUILayout.HelpBox("No SerializableCustomVariable found on the object.", MessageType.Warning);
                GUILayout.EndVertical();
                return;
            }

            string[] varNames = vars.Select(v => v.Name).ToArray();
            selectedIndex = Mathf.Clamp(selectedIndex, 0, varNames.Length - 1);
            selectedIndex = EditorGUILayout.Popup("Variable", selectedIndex, varNames);
            selectedVariableName = varNames[selectedIndex];

            var selectedVar = vars[selectedIndex];
            selectedVariableType = selectedVar.Type;

            GUILayout.Space(5);
            
            // Select CheckType according to custom variable type
            CheckType[] allowedCheckTypes = GetAllowedCheckTypes(selectedVariableType);
            string[] checkTypeNames = allowedCheckTypes.Select(ct => ct.ToString()).ToArray();

            int checkTypeIndex = Mathf.Clamp(Array.IndexOf(allowedCheckTypes, checkType), 0, allowedCheckTypes.Length - 1);
            checkTypeIndex = EditorGUILayout.Popup("Check Type", checkTypeIndex, checkTypeNames);
            checkType = allowedCheckTypes[checkTypeIndex];
            
            GUILayout.Space(5);
            
            valueInputMode = (ValueInputMode)EditorGUILayout.EnumPopup("Value Input Mode", valueInputMode);

            if (valueInputMode == ValueInputMode.EnterValue)
            {
                switch (selectedVariableType)
                {
                    case VariableType.Integer:
                        Debug.Log("HERE 111");
                        int intVal = int.TryParse(valueToCompare, out var iVal) ? iVal : 0;
                        intVal = EditorGUILayout.IntField("Target Value", intVal);
                        valueToCompare = intVal.ToString();
                        Debug.Log("val to comp :" + valueToCompare);
                        break;

                    case VariableType.Float:
                        Debug.Log("HERE 222");
                        float floatVal = float.TryParse(valueToCompare, out var fVal) ? fVal : 0f;
                        floatVal = EditorGUILayout.FloatField("Target Value", floatVal);
                        valueToCompare = floatVal.ToString();
                        break;

                    case VariableType.Boolean:
                        Debug.Log("HERE 333");
                        bool bVal = valueToCompare == "True";
                        bVal = EditorGUILayout.Popup("Target Value", bVal ? 0 : 1, new[] { "True", "False" }) == 0;
                        valueToCompare = bVal.ToString();
                        break;

                    case VariableType.String:
                        Debug.Log("HERE 333");
                        valueToCompare = EditorGUILayout.TextField("Value", valueToCompare);
                        break;
                }
            }
            else if (valueInputMode == ValueInputMode.ValueByReference)
            {
                referenceObject = (EasyObject)EditorGUILayout.ObjectField("Reference Object", referenceObject, typeof(EasyObject), true);

                if (referenceObject != null)
                {
                    Debug.Log("HERE 444");
                    var refVars = referenceObject.GetComponents<SerializableCustomVariable>()
                        .Where(v => v.Type == selectedVariableType)
                        .ToList();

                    if (refVars.Count > 0)
                    {
                        Debug.Log("HERE 555");
                        var refVarNames = refVars.Select(v => v.Name).ToArray();
                        Debug.Log(refVarNames);
                        if (!string.IsNullOrEmpty(referenceVariableName))
                        {
                            Debug.Log("HERE 666");
                            referenceVariableIndex = Array.IndexOf(refVarNames, referenceVariableName);
                        }
                        else
                        {
                            referenceVariableIndex = 0;
                        }
                        if (referenceVariableIndex < 0) referenceVariableIndex = 0;

                        int newRefIndex = EditorGUILayout.Popup("Reference Variable", referenceVariableIndex, refVarNames);
                        // if (newRefIndex != referenceVariableIndex)
                        // {
                        //     Debug.Log("HERE 999");
                        //     // referenceVariableIndex = newRefIndex;
                        //     // referenceVariableName = refVarNames[referenceVariableIndex];
                        //     // referenceVariable = refVars[referenceVariableIndex];
                        //     // valueToCompare = referenceVariable._value;
                        //     //Debug.Log("VALUETOCOMPARE: " + valueToCompare);
                        // }
                        // else
                        // {
                        //     if (referenceVariable != null)
                        //     {
                        //         Debug.Log("HERE 888");
                        //         valueToCompare = referenceVariable._value;
                        //         Debug.Log("VALUETOCOMPARE: " + valueToCompare);
                        //     }
                        // }
                        Debug.Log("HERE 777");
                        referenceVariableIndex = newRefIndex;
                        referenceVariableName = refVarNames[referenceVariableIndex];
                        referenceVariable = refVars[referenceVariableIndex];
                        valueToCompare = referenceVariable._value;
                        Debug.Log("VALUETOCOMPARE: " + valueToCompare);
                    }
                    else
                    {
                        Debug.Log("HERE 999");
                        EditorGUILayout.HelpBox($"No {selectedVariableType} variables found on reference object.", MessageType.Warning);
                        referenceVariableIndex = 0;
                        referenceVariableName = null;
                        referenceVariable = null;
                        valueToCompare = string.Empty;
                    }
                }
                else
                {
                    referenceVariableIndex = 0;
                    referenceVariableName = null;
                    referenceVariable = null;
                    valueToCompare = string.Empty;
                }
            }
            
            Debug.Log("Check Value: " + valueToCompare);

            GUILayout.EndVertical();
        }
        
        private CheckType[] GetAllowedCheckTypes(VariableType varType)
        {
            switch (varType)
            {
                case VariableType.Integer:
                case VariableType.Float:
                    return new[] { CheckType.Equals, CheckType.GreaterThan, CheckType.LessThan };
                case VariableType.String:
                case VariableType.Boolean:
                    return new[] { CheckType.Equals };
                default:
                    return new[] { CheckType.Equals };
            }
        }

        protected override void Subscribe()
        {
            EventBus.OnCustomVariableChanged += OnCustomVariableChanged;
        }

        protected override void Unsubscribe()
        {
            EventBus.OnCustomVariableChanged -= OnCustomVariableChanged;
        }

        private void OnCustomVariableChanged(BaseComponent comp, string varName, object value)
        {
            if (targetObject == null) return;
            Debug.Log("---OnCustomVariableChanged");
            Debug.Log(value);
            var targetComp = targetObject.GetComponent<BaseComponent>();
            if (comp != targetComp) return;
            if (varName != selectedVariableName) return;

            switch (selectedVariableType)
            {
                case VariableType.Integer:
                    Debug.Log("---intttt");
                    Debug.Log("valueToCompare " + valueToCompare);
                    Debug.Log("int: " + value);
                    if (value is int i && int.TryParse(valueToCompare, out var targetInt) && i == targetInt)
                        Trigger(comp);
                    break;

                case VariableType.Float:
                    Debug.Log("---floatttt");
                    if (value is float f && float.TryParse(valueToCompare, out var targetFloat) && Mathf.Approximately(f, targetFloat))
                        Trigger(comp);
                    break;

                case VariableType.Boolean:
                    Debug.Log("---boooll");
                    if (value is bool b && bool.TryParse(valueToCompare, out var targetBool) && b == targetBool)
                        Trigger(comp);
                    break;
                
                case VariableType.String:
                    Debug.Log("---string");
                    if (value is string s && s == valueToCompare)
                        Trigger(comp);
                    break;
            }
        }

        private void Trigger(BaseComponent source)
        {
            foreach (var action in relatedEvent.Actions)
                action.Execute(source, null);
        }
        
        public enum CheckType
        {
            Equals,
            GreaterThan,
            LessThan
        }
        
        public enum ValueInputMode
        {
            EnterValue,
            ValueByReference
        }
    }
}
