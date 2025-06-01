using System.Collections.Generic;
using Backend.EasyEvent;
using UnityEngine;

namespace Backend.Components
{
    public class CollisionEventComponent:BaseComponent
    {
        [SerializeField] private List<(GameObject target, EasyAction action)> _targets = new List<(GameObject, EasyAction)>();
        private Rigidbody2D _rigidbody;
        private BoxCollider2D _boxCollider2D;
        
        public void AddTarget(GameObject obj2, EasyAction action)
        {
            if (_targets.Exists(t => t.target == obj2 && t.action == action))
                return;
            _targets.Add((obj2, action));
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            BaseComponent selfComp  = this;
            BaseComponent otherComp = collision.gameObject.GetComponent<BaseComponent>();
            bool isTriggered = false;

            foreach (var (targetGameObject, action) in _targets)
            {
                if (collision.gameObject == targetGameObject)
                {
                    isTriggered = true;
                    action.Execute(selfComp, otherComp);
                }
            }

            if (isTriggered)
            {
                Destroy(gameObject);
            }
                
        }

        public override void SetupComponent()
        {
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            if (_boxCollider2D == null)
            {
                _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            }
            
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody== null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            }
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}