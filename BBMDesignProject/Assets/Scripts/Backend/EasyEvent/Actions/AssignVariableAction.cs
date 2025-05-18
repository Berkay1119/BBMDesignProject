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
    public class AssignVariableAction : EasyAction
    {
        public SerializableCustomVariable CustomVariable;
        public EasyObject easyObject;
        public string variableName;
        private int variableIndex;
        public string operationValue;
        
        public AssignVariableAction()
        {
            actionName = "AssignVariable";
            actionDescription = "Seçilen objenin custom variable değerini değiştirir.";
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
            operationValue = EditorGUILayout.TextField("Operation Value", operationValue);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (CustomVariable == null) return;
            switch (CustomVariable.Type)
            {
                case VariableType.Integer:
                    if (!int.TryParse(operationValue, out var intVal)) return;
                    CustomVariable._value = intVal.ToString();
                    break;

                case VariableType.Float:
                    if (!float.TryParse(operationValue, out var fOpVal)) return;
                    CustomVariable._value = fOpVal.ToString();
                    break;

                case VariableType.String:
                    CustomVariable._value = operationValue;
                    break;

                case VariableType.Boolean:
                    if (!bool.TryParse(operationValue, out var boolVal)) return;
                    CustomVariable._value = boolVal.ToString();
                    break;
            }
        }
    }
}