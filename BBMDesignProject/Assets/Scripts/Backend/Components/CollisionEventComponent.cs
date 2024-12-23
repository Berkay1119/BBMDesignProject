using System;
using System.Collections.Generic;
using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CollisionEventComponent:BaseComponent
    {
        [SerializeField] private List<(GameObject,EasyAction)> _targets = new List<(GameObject, EasyAction)>();
        public void AddTarget(GameObject obj2,EasyAction action)
        {
            if (_targets.Contains((obj2,action)))
            {
                return;
            }
            _targets.Add((obj2, action));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            foreach (var (targetGameObject, action) in _targets)
            {
                if (other.gameObject == targetGameObject)
                {
                    action.Execute();
                }
            }
            Debug.Log("Collision");
        }

        public override void SetupComponent()
        {
            if (gameObject.GetComponent<BoxCollider>() != null)
            {
                return;
            }
            gameObject.AddComponent<BoxCollider>();
        }
    }
}