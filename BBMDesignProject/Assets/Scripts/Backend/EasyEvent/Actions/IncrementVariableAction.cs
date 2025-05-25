using System.Collections.Generic;
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
            actionDescription = "Increases or decreases the value of a desired custom variable of the selected object.";
        }
        
        public override void DrawGUI()
        {
            base.DrawGUI();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Object:", GUILayout.Width(150));
            easyObject = (EasyObject)EditorGUILayout.ObjectField(easyObject, typeof(EasyObject), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (easyObject != null)
            {
                var customVariables = easyObject.GetComponents<SerializableCustomVariable>();
                var variableNames = customVariables.Select(v => v.Name).ToList();

                if (variableNames.Count > 0)
                {
                    variableIndex = Mathf.Clamp(variableIndex, 0, variableNames.Count - 1);
                    variableIndex = EditorGUILayout.Popup("Variable:", variableIndex, variableNames.ToArray());
                    variableName = variableNames[variableIndex];
                    CustomVariable = customVariables[variableIndex];
                }
                else
                {
                    GUILayout.Label("No variables found.");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            variableOperation = (VariableOperation)EditorGUILayout.EnumPopup("Operation", variableOperation);
            operationValue = EditorGUILayout.TextField("Operation Value", operationValue);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (CustomVariable == null) return;

            string variableValue = CustomVariable._value;
            switch (CustomVariable.Type)
            {
                case VariableType.Integer:
                    if (!int.TryParse(variableValue, out var intVal)) return;
                    if (!int.TryParse(operationValue, out var opVal)) return;

                    switch (variableOperation)
                    {
                        case VariableOperation.Add:
                            intVal += opVal;
                            break;
                        case VariableOperation.Multiply:
                            intVal *= opVal;
                            break;
                        case VariableOperation.Divide:
                            if (opVal == 0) return;
                            intVal /= opVal;
                            break;
                    }

                    CustomVariable._value = intVal.ToString();
                    break;

                case VariableType.Float:
                    if (!float.TryParse(variableValue, out var floatVal)) return;
                    if (!float.TryParse(operationValue, out var fOpVal)) return;
                    
                    switch (variableOperation)
                    {
                        case VariableOperation.Add:
                            floatVal += fOpVal;
                            break;
                        case VariableOperation.Multiply:
                            floatVal *= fOpVal;
                            break;
                        case VariableOperation.Divide:
                            if (Mathf.Approximately(fOpVal, 0f)) return;
                            floatVal /= fOpVal;
                            break;
                    }

                    CustomVariable._value = floatVal.ToString();
                    break;

                case VariableType.String:
                    CustomVariable._value = variableOperation == VariableOperation.Add
                        ? variableValue + operationValue
                        : variableValue.Replace(operationValue, "");
                    break;

                case VariableType.Boolean:
                    if (!bool.TryParse(variableValue, out var boolVal) && variableOperation == VariableOperation.Add) return;
                    CustomVariable._value = (!boolVal).ToString();
                    break;
            }
        }
    }

    public enum VariableOperation
    {
        Add,
        Multiply,
        Divide,
    }
}