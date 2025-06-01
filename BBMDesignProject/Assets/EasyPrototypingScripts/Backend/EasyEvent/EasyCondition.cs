using System;
using UnityEngine;

namespace Backend.EasyEvent
{
    [Serializable]
    public abstract class EasyCondition
    {
        public string conditionName;
        public string conditionDescription;
        
        [NonSerialized] [HideInInspector]
        internal EasyEvent relatedEvent;
        
        protected bool _isSubscribed;

        public virtual void DrawGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Condition Name: " + conditionName);
        }

        public virtual void Setup(EasyEvent easyEvent)
        {
            if (_isSubscribed)
            {
                Unsubscribe();
            }
            relatedEvent = easyEvent;
            Subscribe();
            _isSubscribed = true;
        }
        
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}