using System;
using System.Collections.Generic;
using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class PlatformAvatarController:BaseComponent,IUpdatable
    {
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;

        private Rigidbody2D _currentPlatformRb;
        private Rigidbody2D _rigidbody2D;

        public override void SetupComponent()
        {
            
        }
        
        public PlatformAvatarController()
        {
            SetName("Platform Avatar Controller");
            SetDescription("This component allows the object to move on platforms and jump.");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D==null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody2D.gravityScale = 1f; // Default gravity scale
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
            
            var platformVel = _currentPlatformRb != null
                ? _currentPlatformRb.velocity
                : Vector2.zero;
            
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.x = inputVel.x + platformVel.x;
            velocity.y = velocity.y + inputVel.y + platformVel.y;
            
            if (Input.GetKeyDown(jumpKey) && _currentPlatformRb != null)
            {
                velocity.y = jumpForce;
            }
            
            _rigidbody2D.velocity = velocity;
        }
    }
}