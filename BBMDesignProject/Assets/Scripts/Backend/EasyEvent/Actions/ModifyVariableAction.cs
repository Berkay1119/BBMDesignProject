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
            actionDescription = "Seçilen objenin custom variable değerini değiştirir";
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
                        case VariableOperation.Increment:
                            intVal += opVal;
                            break;
                        case VariableOperation.Decrement:
                            intVal -= opVal;
                            break;
                    }

                    CustomVariable._value = intVal.ToString();
                    break;

                case VariableType.Float:
                    if (!float.TryParse(variableValue, out var floatVal)) return;
                    if (!float.TryParse(operationValue, out var fOpVal)) return;
                    floatVal = variableOperation == VariableOperation.Increment ? floatVal + fOpVal : floatVal - fOpVal;
                    CustomVariable._value = floatVal.ToString();
                    break;

                case VariableType.String:
                    // Örneğin birleştir
                    CustomVariable._value = variableOperation == VariableOperation.Increment
                        ? variableValue + operationValue
                        : variableValue.Replace(operationValue, "");
                    break;

                case VariableType.Boolean:
                    if (!bool.TryParse(variableValue, out var boolVal)) return;
                    // toggle dışında opValue'yu parse edip setleyebilirsin
                    CustomVariable._value = (!boolVal).ToString();
                    break;
            }
        }
    }

    public enum VariableOperation
    {
        Increment,
        Decrement,
    }
}