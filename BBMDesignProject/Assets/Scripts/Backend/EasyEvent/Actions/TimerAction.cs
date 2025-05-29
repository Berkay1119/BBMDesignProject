using Backend.Attributes;
using Backend.Components;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    public enum TimerActionType
    {
        Start,
        Stop
    }
    
    [Action]
    public class TimerAction:EasyAction
    {
        public TimerComponent TimerComponent;
        public TimerActionType ActionType;
        
        public TimerAction()
        {
            actionName = "Timer Action";
            actionDescription = "Start or stop a timer.";
        }
        public override void Execute(BaseComponent source, BaseComponent other)
        {
            if (TimerComponent == null)
            {
                Debug.LogError("TimerComponent is not assigned in TimerAction.");
                return;
            }

            if (ActionType == TimerActionType.Start)
            {
                TimerComponent.StartTimer();
            }
            else if (ActionType == TimerActionType.Stop)
            {
                TimerComponent.StopTimer();
            }
        }
        
        public override void DrawGUI()
        {
            base.DrawGUI();
            TimerComponent = (TimerComponent)UnityEditor.EditorGUILayout.ObjectField("Timer Component", TimerComponent, typeof(TimerComponent), true);
            ActionType = (TimerActionType)UnityEditor.EditorGUILayout.EnumPopup("Action Type", ActionType);
            GUILayout.EndVertical();
        }
    }
}