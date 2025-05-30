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
        public ValueInputMode valueInputMode = ValueInputMode.EnterValue;
        public EasyObject referenceEasyObject;
        public SerializableCustomVariable referenceCustomVariable;
        public string referenceVariableName;
        public VariableOperation variableOperation;
        public string operationValue;
        
        private int variableIndex;
        private int referenceVariableIndex = 0;
        
        public ModifyVariableAction()
        {
            actionName = "ModifyVariable";
            actionDescription = "Modifies or sets the value of a custom variable based on its type and selected operation.";
        }
        
        public override void DrawGUI()
        {
            GUILayout.Space(10);
            base.DrawGUI();
            GUILayout.Space(10);
            
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
                        valueInputMode = ValueInputMode.EnterValue;

                        // Reset reference values
                        referenceEasyObject = null;
                        referenceVariableName = null;
                        referenceCustomVariable = null;
                        referenceVariableIndex = 0;
                    }
                    else
                    {
                        variableName = variableNames[variableIndex];
                        CustomVariable = customVariables[variableIndex];
                    }

                    DrawOperationGUI(CustomVariable.Type);
                    
                    if (OperationNeedsValueInput(variableOperation))
                    {
                        //valueInputMode = (ValueInputMode)EditorGUILayout.EnumPopup("Value Input Mode", valueInputMode);
                        
                        // Check if the valueInputMode changed
                        var newValueInputMode = (ValueInputMode)EditorGUILayout.EnumPopup("Value Input Mode", valueInputMode);

                        // If the ValueInputMode changes, clear the respective fields
                        if (newValueInputMode != valueInputMode)
                        {
                            if (newValueInputMode == ValueInputMode.EnterValue)
                            {
                                // Clear operation value when switching to EnterValue
                                operationValue = "";
                            }
                            else if (newValueInputMode == ValueInputMode.ValueByReference)
                            {
                                // Clear reference object and reference variable when switching to ValueByReference
                                referenceEasyObject = null;
                                referenceVariableName = null;
                                referenceCustomVariable = null;
                            }

                            valueInputMode = newValueInputMode;
                        }
                        
                        if (valueInputMode == ValueInputMode.EnterValue)
                        {
                            operationValue = EditorGUILayout.TextField("Value", operationValue);
                        }
                        else if (valueInputMode == ValueInputMode.ValueByReference)
                        {
                            referenceEasyObject = (EasyObject)EditorGUILayout.ObjectField("Reference Object", referenceEasyObject, typeof(EasyObject), true);

                            if (referenceEasyObject != null)
                            {
                                Debug.Log("--- hereeeğeğeğe");
                                var refVariables = referenceEasyObject.GetComponents<SerializableCustomVariable>()
                                    .Where(v => v.Type == CustomVariable.Type)
                                    .ToList();

                                if (refVariables.Count > 0)
                                {
                                    Debug.Log("--- aaaaaaa");
                                    var refVarNames = refVariables.Select(v => v.Name).ToArray();

                                    if (!string.IsNullOrEmpty(referenceVariableName))
                                    {
                                        Debug.Log("--- bbbbbbb");
                                        referenceVariableIndex = Array.IndexOf(refVarNames, referenceVariableName);
                                    }
                                    else
                                    {
                                        Debug.Log("--- cccccccc");
                                        referenceVariableIndex = 0;
                                    }
                                    if (referenceVariableIndex < 0) referenceVariableIndex = 0;

                                    int newRefVarIndex = EditorGUILayout.Popup("Reference Variable", referenceVariableIndex, refVarNames);
                                    Debug.Log("--- newRefVarIndex " + newRefVarIndex);
                                    
                                    referenceVariableIndex = newRefVarIndex;
                                    referenceVariableName = refVarNames[referenceVariableIndex];
                                    referenceCustomVariable = refVariables[referenceVariableIndex];
                                    
                                    Debug.Log("name: " + referenceVariableName);
                                }
                                else
                                {
                                    EditorGUILayout.HelpBox($"No {CustomVariable.Type} variables found on reference object.", MessageType.Warning);
                                    referenceVariableName = null;
                                    referenceCustomVariable = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // No need for operation value
                        operationValue = "";
                        valueInputMode = ValueInputMode.EnterValue;

                        // Clean references
                        referenceEasyObject = null;
                        referenceVariableName = null;
                        referenceCustomVariable = null;
                        referenceVariableIndex = 0;
                    }
                    
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
        
        private bool OperationNeedsValueInput(VariableOperation op)
        {
            //Checks if an operation value needed or not
            if (op == VariableOperation.Invert)
            {
                return false;
            }
            return true;
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
                    break;

                case VariableType.Boolean:
                    string[] boolOps = { "Set", "Invert", "SetNot"};
                    int selected = variableOperation switch
                    {
                        VariableOperation.Set => 0,
                        VariableOperation.Invert => 1,
                        VariableOperation.SetNot => 2,
                        _ => 0 // Default to "Set" if none selected
                    };
                    selected = EditorGUILayout.Popup("Operation", selected, boolOps);
                    variableOperation = selected switch
                    {
                        0 => VariableOperation.Set,
                        1 => VariableOperation.Invert,
                        2 => VariableOperation.SetNot,
                        _ => variableOperation
                    };                    
                    break;

                case VariableType.String:
                    variableOperation = VariableOperation.Set;
                    break;
            }
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (CustomVariable == null) return;
            Debug.Log("MODIFY VARIABLE EXECUTE");
            BaseComponent component = easyObject.GetComponent<BaseComponent>();
            if (component == null) return;
            Debug.Log("MODIFY VARIABLE EXECUTE");
            string currentValue = CustomVariable._value;
            string inputValue = "";
            
            try
            {
                if (OperationNeedsValueInput(variableOperation))
                {
                    Debug.Log("HERE 11");
                    if (valueInputMode == ValueInputMode.EnterValue)
                    {
                        Debug.Log("HERE 22");
                        inputValue = operationValue;
                    }
                    else if (valueInputMode == ValueInputMode.ValueByReference)
                    {
                        Debug.Log("HERE 33");
                        if (referenceEasyObject == null || referenceCustomVariable == null)
                        {
                            if (referenceEasyObject == null)
                            {
                                Debug.Log("HERE 44");
                            }

                            if (referenceCustomVariable == null)
                            {
                                Debug.Log("HERE 55");
                            }
                            Debug.LogWarning("[ModifyVariableAction] Reference object or variable is not set.");
                            return;
                        }
                        Debug.Log("HERE 66");
                        inputValue = referenceCustomVariable._value;
                        Debug.Log("--- VALUE: " + inputValue);
                    }
                }
                
                switch (CustomVariable.Type)
                {
                    case VariableType.Integer:
                        int iVal = int.TryParse(currentValue, out var iParsed) ? iParsed : 0;
                        int iInput = int.TryParse(inputValue, out var iOp) ? iOp : 0;
                        Debug.Log("--- iInput VALUE: " + iInput);
                        
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
                        float fInput = float.TryParse(inputValue, out var fOp) ? fOp : 0f;

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
                            VariableOperation.Set => bool.TryParse(inputValue, out var bSet) && bSet,
                            VariableOperation.SetNot => !(bool.TryParse(inputValue, out var refVal) && refVal), 
                            _ => bVal
                        };
                        CustomVariable._value = newBoolVal.ToString();
                        EventBus.PublishCustomVariableChanged(component, variableName, newBoolVal);
                        break;

                    case VariableType.String:
                        CustomVariable._value = inputValue;
                        EventBus.PublishCustomVariableChanged(component, variableName, operationValue);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[ModifyVariableAction] Operation failed: {e.Message}");
            }

        }
    }
    
    public enum ValueInputMode
    {
        EnterValue,
        ValueByReference
    }

    public enum VariableOperation
    {
        // Numeric, String
        Add,
        Multiply,
        Divide,
        Set,

        // Bool-specific
        Invert,
        SetNot
    }
}