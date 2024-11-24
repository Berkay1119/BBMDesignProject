using System;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public class EasyEvent
    {
        public string eventName;
        public string eventDescription;
        public EasyCondition Condition;
        public EasyAction Action;
    }
}