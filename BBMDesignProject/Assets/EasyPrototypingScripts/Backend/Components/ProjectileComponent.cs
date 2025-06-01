using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Projectile Component")]
    public class ProjectileComponent : BaseComponent
    {
        private Vector2 _startPosition;
        private Vector2 _direction;
        private float _speed = 10f; 
        private Rigidbody2D _rigidbody2D;
        private GameObject _originGameObject;
        
        public override void SetupComponent()
        {

        }
        
        public ProjectileComponent()
        {
            SetName("Projectile");
            SetDescription("This component represents a projectile that can be thrown in a specified direction.");
        }
        
        public void Initialize(GameObject originGameObject, Vector2 startPosition, Vector2 direction, float speed)
        {
            if (gameObject.GetComponent<Rigidbody2D>() == null )
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }

            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0; // Disable gravity for the projectile

            if (gameObject.GetComponent<BoxCollider2D>() == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
            
            _startPosition = startPosition;
            _direction = direction.normalized;

            transform.position = _startPosition;
            
            //_rigidbody2D.isKinematic = true;
            _rigidbody2D.velocity = _direction * speed; 
            
            _originGameObject = originGameObject;
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject == _originGameObject)
            {
                // If the collision is with its own source, do nothing
                return;
            }

            base.OnCollisionEnter2D(col);
        }
        
    }
}