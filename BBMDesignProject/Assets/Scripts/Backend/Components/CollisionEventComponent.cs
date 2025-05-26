using System.Collections.Generic;
using Backend.EasyEvent;
using UnityEngine;

namespace Backend.Components
{
    public class CollisionEventComponent:BaseComponent
    {
        [SerializeField] private List<(GameObject target, EasyAction action)> _targets = new List<(GameObject, EasyAction)>();
        
        public void AddTarget(GameObject obj2, EasyAction action)
        {
            if (_targets.Exists(t => t.target == obj2 && t.action == action))
                return;
            _targets.Add((obj2, action));
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // Collision’dan hem BaseComponent'imizi hem de diğer objenin BaseComponent'ini alalım
            BaseComponent selfComp  = this;
            BaseComponent otherComp = collision.gameObject.GetComponent<BaseComponent>();
            bool isTriggered = false;

            foreach (var (targetGameObject, action) in _targets)
            {
                if (collision.gameObject == targetGameObject)
                {
                    isTriggered = true;
                    // Yeni imzaya uygun çağrı
                    action.Execute(selfComp, otherComp);
                }
            }

            if (isTriggered)
                Destroy(gameObject);
        }

        public override void SetupComponent()
        {
            // 2D ortam için BoxCollider2D ekliyoruz
            if (gameObject.GetComponent<BoxCollider2D>() == null)
                gameObject.AddComponent<BoxCollider2D>();
            // Rigidbody2D gerekebilir:
            if (gameObject.GetComponent<Rigidbody2D>() == null)
            {
                var rb = gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
}