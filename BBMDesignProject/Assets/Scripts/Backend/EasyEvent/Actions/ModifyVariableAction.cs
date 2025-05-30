using System;
using System.Linq;
using Backend.Attributes;
using Backend.Components;
using Backend.CustomVariableFeature;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    [Action]
    public class ModifyVariableAction : EasyAction
    {
        public SerializableCustomVariable CustomVariable;
        public EasyObject easyObject;
        public string variableName;
        private int variableIndex;
        public VariableOperation variableOperation;
        public string operationValue;
        
        public ModifyVariableAction()
        {
            actionName = "ModifyVariable";
            actionDescription = "Modifies or sets the value of a custom variable based on its type and selected operation.";
        }
        
        public override void DrawGUI()
        {
            base.DrawGUI();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Object:", GUILayout.Width(150));
            easyObject = (EasyObject)EditorGUILayout.ObjectField(easyObject, typeof(EasyObject), true);
            GUILayout.EndHorizontal();

            if (easyObject != null)
            {
                var customVariables = easyObject.GetComponents<SerializableCustomVariable>();
                var variableNames = customVariables.Select(v => v.Name).ToList();

                if (variableNames.Count > 0)
                {
                    if (!string.IsNullOrEmpty(variableName))
                    {
                        variableIndex = variableNames.IndexOf(variableName);
                    }
                    else
                    {
                        variableIndex = 0;
                    }

                    if (variableIndex < 0) variableIndex = 0;
                    int newIndex = EditorGUILayout.Popup("Variable:", variableIndex, variableNames.ToArray());

                    if (newIndex != variableIndex)
                    {
                        variableIndex = newIndex;
                        variableName = variableNames[variableIndex];
                        CustomVariable = customVariables[variableIndex];

                        // Reset operation value only if variable changed
                        operationValue = "";
                    }
                    else
                    {
                        variableName = variableNames[variableIndex];
                        CustomVariable = customVariables[variableIndex];
                    }

                    DrawOperationGUI(CustomVariable.Type);
                }
                else
                {
                    GUILayout.Label("No variables found.");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select an object to modify a variable of it.", MessageType.Info);
            }
            
            GUILayout.EndVertical();
        }
        
        private void DrawOperationGUI(VariableType type)
        {
            GUILayout.Space(10);
            switch (type)
            {
                case VariableType.Integer:
                case VariableType.Float:
                    string[] numericOps = { "Add", "Multiply", "Divide", "Set" };
                    int selectedNumeric = (int)variableOperation;
                    if (selectedNumeric > 3) selectedNumeric = 0; // Fallback from Invert etc.
                    selectedNumeric = EditorGUILayout.Popup("Operation", selectedNumeric, numericOps);
                    variableOperation = (VariableOperation)selectedNumeric;

                    operationValue = EditorGUILayout.TextField("Value", operationValue);
                    break;

                case VariableType.Boolean:
                    string[] boolOps = { "Set", "Invert" };
                    int selected = variableOperation == VariableOperation.Set ? 0 : 1;
                    selected = EditorGUILayout.Popup("Operation", selected, boolOps);
                    variableOperation = selected == 0 ? VariableOperation.Set : VariableOperation.Invert;

                    if (variableOperation == VariableOperation.Set)
                    {
                        int boolVal = operationValue == "True" ? 0 : 1;
                        boolVal = EditorGUILayout.Popup("Value", boolVal, new[] { "True", "False" });
                        operationValue = boolVal == 0 ? "True" : "False";
                    }
                    break;

                case VariableType.String:
                    variableOperation = VariableOperation.Set;
                    operationValue = EditorGUILayout.TextField("Value", operationValue);
                    break;
            }
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (CustomVariable == null) return;
            
            BaseComponent component = easyObject.GetComponent<BaseComponent>();
            if (component == null) return;
            
            string currentValue = CustomVariable._value;
            
            try
            {
                switch (CustomVariable.Type)
                {
                    case VariableType.Integer:
                        int iVal = int.TryParse(currentValue, out var iParsed) ? iParsed : 0;
                        int iInput = int.TryParse(operationValue, out var iOp) ? iOp : 0;

                        CustomVariable._value = variableOperation switch
                        {
                            VariableOperation.Add => (iVal + iInput).ToString(),
                            VariableOperation.Multiply => (iVal * iInput).ToString(),
                            VariableOperation.Divide => iInput == 0 ? iVal.ToString() : (iVal / iInput).ToString(),
                            VariableOperation.Set => iInput.ToString(),
                            _ => currentValue
                        };
                        EventBus.PublishCustomVariableChanged(component, variableName, int.Parse(CustomVariable._value));
                        break;

                    case VariableType.Float:
                        float fVal = float.TryParse(currentValue, out var fParsed) ? fParsed : 0f;
                        float fInput = float.TryParse(operationValue, out var fOp) ? fOp : 0f;

                        CustomVariable._value = variableOperation switch
                        {
                            VariableOperation.Add => (fVal + fInput).ToString(),
                            VariableOperation.Multiply => (fVal * fInput).ToString(),
                            VariableOperation.Divide => Mathf.Approximately(fInput, 0f) ? fVal.ToString() : (fVal / fInput).ToString(),
                            VariableOperation.Set => fInput.ToString(),
                            _ => currentValue
                        };
                        EventBus.PublishCustomVariableChanged(component, variableName, float.Parse(CustomVariable._value));
                        break;

                    case VariableType.Boolean:
                        bool.TryParse(currentValue, out var bVal);
                        bool newBoolVal = variableOperation switch
                        {
                            VariableOperation.Invert => !bVal,
                            VariableOperation.Set => bool.TryParse(operationValue, out var bSet) && bSet,
                            _ => bVal
                        };
                        CustomVariable._value = newBoolVal.ToString();
                        EventBus.PublishCustomVariableChanged(component, variableName, newBoolVal);
                        break;

                    case VariableType.String:
                        CustomVariable._value = operationValue;
                        EventBus.PublishCustomVariableChanged(component, variableName, operationValue);
                        break;
                }

                CustomVariable.ValueChanged();
            }
            catch (Exception e)
            {
                Debug.LogError($"[ModifyVariableAction] Operation failed: {e.Message}");
            }

        }
    }

    public enum VariableOperation
    {
        // Numeric, String
        Add,
        Multiply,
        Divide,
        Set,

        // Bool-specific
        Invert
    }
}