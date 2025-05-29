using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Platform Movement Component")]
    public class PlatformMovementController:BaseComponent,IUpdatable
    {
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;

        private Rigidbody2D _platformRb;
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;
        private bool isGrounded = false;

        public PlatformMovementController()
        {
            SetName("Platform Movement Controller");
            SetDescription("This component allows controlling the object in platformer games");
        }
        
        public override void SetupComponent()
        {
            
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D==null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }
            // Default gravity scale
            _rigidbody2D.gravityScale = 1f; 
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            if (_boxCollider2D == null)
            {
                _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            if (col.gameObject.GetComponent<SolidComponent>() != null)
            {
                isGrounded = true;
                _platformRb = col.rigidbody;
            }
            
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<SolidComponent>() != null)
            {
                isGrounded = false;
                _platformRb = null;
            }
        }
        

        public void OnUpdate()
        {
            Vector2 inputVel = Vector2.zero;
            
            if (Input.GetKey(leftKey))
            {
                inputVel = Vector2.left * moveSpeed;
            }
            else if (Input.GetKey(rightKey))
            {
                inputVel = Vector2.right * moveSpeed;
            }
            
            
            var platformVel = _platformRb != null
                ? _platformRb.velocity
                : Vector2.zero;
            
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.x = inputVel.x + platformVel.x;

            if (isGrounded)
            {
                velocity.y = platformVel.y;
            }
            else
            {
                velocity.y = velocity.y + inputVel.y;
            }
            
            
            if (Input.GetKeyDown(jumpKey) && _platformRb != null)
            {
                velocity.y = jumpForce;
                isGrounded = false;
            }
            
            _rigidbody2D.velocity = velocity;
        }
    }
}