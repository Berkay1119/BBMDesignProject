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
        [Header("Colliding")]
        public bool isColliding;
        public List<GameObject> collidingObjects = new List<GameObject>();
    }
}