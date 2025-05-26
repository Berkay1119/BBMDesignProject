using Backend.Attributes;
using Backend.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.EasyEvent.Actions
{
    [Action]
    public class InvokeAction:EasyAction
    {
        public CustomEventScriptableObject onInvoke;

        public InvokeAction()
        {
            actionName = "Invoke Action";
            actionDescription = "This action invokes the UnityEvent when executed.";
        }

        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (onInvoke != null)
            {
                onInvoke.Raise();
            }
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            onInvoke = (CustomEventScriptableObject)UnityEditor.EditorGUILayout.ObjectField("On Invoke", onInvoke, typeof(CustomEventScriptableObject), false);
            GUILayout.EndVertical();
        }
    }
}