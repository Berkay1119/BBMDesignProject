using System;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public class EasyEvent
    {
        public string eventName;
        public string eventDescription;
        [SerializeReference] public EasyCondition Condition;
        [SerializeReference] public EasyAction Action;

        public void Setup()
        {
            Condition.Setup(Action);
        }
    }
}