using System.Collections.Generic;
using Backend.Attributes;
using Backend.CustomVariableFeature;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    [Action]
    public class ModifyVariableAction:EasyAction
    {
        public SerializableCustomVariable CustomVariable;
        public EasyObject easyObject;
        public string variableName;
        private int variableIndex;
        public VariableOperation variableOperation;
        public string operationValue;
        public override void DrawGUI()
        {
            base.DrawGUI();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Object:",GUILayout.Width(150));
            easyObject = (EasyObject) EditorGUILayout.ObjectField(easyObject, typeof(EasyObject), true);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (easyObject != null)
            {
                List<string> variableNames = new List<string>();
                var customVariables = easyObject.GetComponents<SerializableCustomVariable>();
                foreach (SerializableCustomVariable customVariable in customVariables)
                {
                    
                    variableNames.Add(customVariable.Name);
                }
            
                if (variableNames.Count > 0)
                {
                    // Ensure selectedIndex is maintained as a class-level variable
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
            variableOperation = (VariableOperation) EditorGUILayout.EnumPopup("Operation",variableOperation);
            operationValue = EditorGUILayout.TextField("OperationValue:",operationValue);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public override void Execute()
        {
            string variableValue = CustomVariable._value;
            switch (CustomVariable.Type)
            {
                case VariableType.Integer:
                    int variableInt = int.Parse(variableValue);
                    switch (variableOperation)
                    {
                        case VariableOperation.Increment:
                            variableInt += int.Parse(operationValue);
                            break;
                        case VariableOperation.Decrement:
                            variableInt -= int.Parse(operationValue);
                            break;
                        default:
                            break;
                    }
                    CustomVariable._value = variableInt.ToString();
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