using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend.EasyEvent
{
    [Serializable]
    public class EasyEvent
    {
        public string eventName;
        public string eventDescription;
        [SerializeReference] public List<EasyCondition> Conditions = new List<EasyCondition>();
        [SerializeReference] public List<EasyAction> Actions = new List<EasyAction>();

        public void Setup()
        {
            foreach (var condition in Conditions)
            {
                condition.Setup(this);
            }
        }
        
        
    }
}