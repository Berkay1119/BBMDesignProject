using UnityEngine;

namespace Backend.Components
{
    public abstract class ProjectileComponent : BaseComponent
    {
        private Vector2 _startPosition;
        private Vector2 _direction;
        private float _speed = 10f; 
        private Rigidbody2D _rigidbody2D;
        private GameObject _originGameObject;
        
        public override void SetupComponent()
        {
            if (gameObject.GetComponent<Rigidbody2D>() == null )
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }

            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        }
        
        public void Initialize(GameObject originGameObject, Vector2 startPosition, Vector2 direction, float speed)
        {
            SetupComponent();
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