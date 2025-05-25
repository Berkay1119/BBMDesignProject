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

        public CheckValueCondition()
        {
            conditionName = "Check Value Condition";
            conditionDescription = "Checks if a selected variable on a object matches the desired value.";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();

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
            switch (selectedVariableType)
            {
                case VariableType.Integer:
                    int intVal = int.TryParse(valueToCompare, out var iVal) ? iVal : 0;
                    intVal = EditorGUILayout.IntField("Target Value", intVal);
                    valueToCompare = intVal.ToString();
                    break;

                case VariableType.Float:
                    float floatVal = float.TryParse(valueToCompare, out var fVal) ? fVal : 0f;
                    floatVal = EditorGUILayout.FloatField("Target Value", floatVal);
                    valueToCompare = floatVal.ToString();
                    break;

                case VariableType.Boolean:
                    bool bVal = valueToCompare == "True";
                    bVal = EditorGUILayout.Popup("Target Value", bVal ? 0 : 1, new[] { "True", "False" }) == 0;
                    valueToCompare = bVal.ToString();
                    break;

                default:
                    EditorGUILayout.HelpBox("Unsupported variable type.", MessageType.Warning);
                    break;
            }

            GUILayout.EndVertical();
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

            var targetComp = targetObject.GetComponent<BaseComponent>();
            if (comp != targetComp) return;
            if (varName != selectedVariableName) return;

            switch (selectedVariableType)
            {
                case VariableType.Integer:
                    if (value is int i && int.TryParse(valueToCompare, out var targetInt) && i == targetInt)
                        Trigger(comp);
                    break;

                case VariableType.Float:
                    if (value is float f && float.TryParse(valueToCompare, out var targetFloat) && Mathf.Approximately(f, targetFloat))
                        Trigger(comp);
                    break;

                case VariableType.Boolean:
                    if (value is bool b && bool.TryParse(valueToCompare, out var targetBool) && b == targetBool)
                        Trigger(comp);
                    break;
            }
        }

        private void Trigger(BaseComponent source)
        {
            foreach (var action in relatedEvent.Actions)
                action.Execute(source, null);
        }
    }
}
