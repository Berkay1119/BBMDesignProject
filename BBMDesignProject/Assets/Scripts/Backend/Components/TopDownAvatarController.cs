using System.Collections.Generic;
using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class TopDownAvatarController:BaseComponent,IUpdatable
    {
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode upKey = KeyCode.W;
        [SerializeField] private KeyCode downKey = KeyCode.S;
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D _currentPlatformRb;
        private Rigidbody2D _rigidbody2D;

        public override void SetupComponent()
        {
            
        }
        
        public TopDownAvatarController()
        {
            SetName("Top Down Avatar Controller");
            SetDescription("This component allows the object to move in a top-down manner.");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D==null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody2D.gravityScale = 0f; // Default gravity scale
            }
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            BoxCollider2D tempCollider = gameObject.GetComponent<BoxCollider2D>();
            if (tempCollider == null)
            {
                tempCollider = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            if (col.gameObject.CompareTag("Platform"))
            {
                _currentPlatformRb = col.rigidbody;
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Platform"))
            {
                _currentPlatformRb = null;
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
            else if (Input.GetKey(upKey))
            {
                inputVel = Vector2.up * moveSpeed;
            }
            else if (Input.GetKey(downKey))
            {
                inputVel = Vector2.down * moveSpeed;
            }

            Vector2 velocity = _rigidbody2D.velocity;
            
            velocity = inputVel;

            _rigidbody2D.velocity = velocity;
        }
    }
}