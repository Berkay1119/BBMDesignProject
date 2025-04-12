using System;
using UnityEngine;

namespace Backend.EasyEvent
{
    [Serializable]
    public abstract class EasyCondition
    {
        public string conditionName;
        public string conditionDescription;
        public EasyEvent relatedEvent;

        public virtual void DrawGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Condition Name: " + conditionName);
        }

        public virtual void Setup(EasyEvent easyEvent)
        {
            this.relatedEvent = easyEvent;
        }
        
        public abstract bool Check();
    }
}