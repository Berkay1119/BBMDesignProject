using System;
using System.Collections.Generic;
using Backend.Attributes;
using Backend.CustomVariableFeature;
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
       
        [SerializeField] private SerializableCustomVariable moveSpeedVariable;
        [SerializeField] private SerializableCustomVariable jumpForceVariable;
        
        private float MoveSpeed => ParseFloatVariable(moveSpeedVariable, 0f);
        private float JumpForce => ParseFloatVariable(jumpForceVariable, 0f);
        
        private List<JumpableComponent> _jumpableComponents = new List<JumpableComponent>();

        private float ParseFloatVariable(SerializableCustomVariable variable, float defaultValue)
        {
            if (variable == null)
            {
                return defaultValue;
            }

            if (float.TryParse(variable._value, out float result))
            {
                return result;
            }
            
            return defaultValue;
        }
        
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

        private void FixedUpdate()
        {
            for (int i = 0; i < _jumpableComponents.Count; i++)
            {
                var component = _jumpableComponents[i];
                if (component==null || !component.gameObject.activeInHierarchy)
                {
                    OnSimulatedCollisionExit(); 
                    _jumpableComponents.RemoveAt(i);
                    i--; // Adjust index after removal
                }
            }
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
            
            if (col.gameObject.GetComponent<JumpableComponent>() != null)
            {
                isGrounded = true;
                _platformRb = col.rigidbody;
                if (!_jumpableComponents.Contains(col.gameObject.GetComponent<JumpableComponent>()))
                {
                    _jumpableComponents.Add(col.gameObject.GetComponent<JumpableComponent>());
                }
            }
            
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<JumpableComponent>() != null)
            {
                isGrounded = false;
                _platformRb = null;
                var jumpableComponent = col.gameObject.GetComponent<JumpableComponent>();
                if (_jumpableComponents.Contains(jumpableComponent))
                {
                    _jumpableComponents.Remove(jumpableComponent);
                }
            }
        }

        private void OnSimulatedCollisionExit()
        {
            isGrounded = false;
            _platformRb = null;
        }
        

        public void OnUpdate()
        {
            Vector2 inputVel = Vector2.zero;
            
            if (Input.GetKey(leftKey))
            {
                inputVel = Vector2.left * MoveSpeed;
            }
            else if (Input.GetKey(rightKey))
            {
                inputVel = Vector2.right * MoveSpeed;
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
            
            
            if (Input.GetKeyDown(jumpKey) && isGrounded)
            {
                velocity.y = JumpForce;
                isGrounded = false;
            }
            
            _rigidbody2D.velocity = velocity;
        }
    }
}