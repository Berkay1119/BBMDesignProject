using System;
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
        }

        public abstract void Execute();
    }
}