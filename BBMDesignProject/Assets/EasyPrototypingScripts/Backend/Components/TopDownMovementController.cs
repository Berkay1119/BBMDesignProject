﻿using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Top Down Movement Component")]
    public class TopDownMovementController:BaseComponent,IUpdatable
    {
        [SerializeField] private KeyCode leftKey = KeyCode.A;
        [SerializeField] private KeyCode rightKey = KeyCode.D;
        [SerializeField] private KeyCode upKey = KeyCode.W;
        [SerializeField] private KeyCode downKey = KeyCode.S;
        [SerializeField] private float moveSpeed = 5f;
        
        private Rigidbody2D _rigidbody2D;

        public override void SetupComponent()
        {
            
        }
        
        public TopDownMovementController()
        {
            SetName("Top Down Movement Controller");
            SetDescription("This component allows the object to move in a top-down manner.");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            
            if (_rigidbody2D==null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic; // Use dynamic for top-down movement
            _rigidbody2D.gravityScale = 0f; // Default gravity scale
            
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            BoxCollider2D tempCollider = gameObject.GetComponent<BoxCollider2D>();
            
            if (tempCollider == null)
            {
                tempCollider = gameObject.AddComponent<BoxCollider2D>();
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