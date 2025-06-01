using System;
using Backend.Components;
using UnityEngine;

namespace Backend.EasyEvent
{
    [Serializable]
    public abstract class EasyAction
    {
        public string actionName;
        public string actionDescription;

        public virtual void DrawGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Action Name: " + actionName);
            GUILayout.Label("Action Description: " + actionDescription);
        }

        public abstract void Execute(BaseComponent source, BaseComponent other);
        
        public virtual void Execute(BaseComponent component)
        {
            Execute(component, null);
        }
    }
}