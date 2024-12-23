using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public abstract class EasyCondition
    {
        public string conditionName;
        public string conditionDescription;

        public virtual void DrawGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Condition Name: " + conditionName);
        }

        public abstract void Setup(EasyAction action);
    }
}