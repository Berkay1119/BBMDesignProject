using System;
using System.Collections.Generic;
using Backend.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Backend.Components.SubComponents
{
    public class PlayerControllerComponent : BaseComponent, IUpdatable
    {
        private CharacterComponent _characterComponent;
        private Rigidbody2D _rigidbody2D;
        private Rigidbody2D _currentPlatformRb;
        
        public  ProjectileThrower projectileThrower;
        
        private readonly Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();
        
        private bool isTopDown = false;
        private Transform _lookAtTransform;

        private KeyCode GetKeyCode(char character)
        {
            KeyCode code;
            if (_keycodeCache.TryGetValue(character, out code)) return code;

            // Cast to it's integer value
            int alphaValue = character;
            code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
            _keycodeCache.Add(character, code);
            return code;
        }
        
        public void Setup(CharacterComponent characterComponent, Rigidbody2D rigidbody2D, bool isTopDown = false)
        {
            _characterComponent = characterComponent;
            _rigidbody2D = rigidbody2D;
            this.isTopDown = isTopDown;
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
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

        public void OnUpdate() {
            if (!_characterComponent) return;
            
            Vector2 inputVel = Vector2.zero;
            if (Input.GetKey(GetKeyCode(_characterComponent.LeftKey)))
                inputVel = Vector2.left*_characterComponent.Speed;
            else if (Input.GetKey(GetKeyCode(_characterComponent.RightKey)))
                inputVel = Vector2.right*_characterComponent.Speed;
            else if (isTopDown)
            {
                if (Input.GetKey(GetKeyCode(_characterComponent.UpKey)))
                {
                    inputVel = Vector2.up*_characterComponent.Speed;
                }
                else if (Input.GetKey(GetKeyCode(_characterComponent.DownKey)))
                {
                    inputVel = Vector2.down*_characterComponent.Speed;
                }
            }

            // Platform speed
            var platformVel = _currentPlatformRb != null
                ? _currentPlatformRb.velocity
                : Vector2.zero;

            // Vertical speed
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.y = velocity.y + inputVel.y + platformVel.y;
            velocity.x = inputVel.x + platformVel.x;
            // Jump if the character on a platform
            if (Input.GetKeyDown(KeyCode.Space) && _currentPlatformRb != null)
            {
                velocity.y = _characterComponent.JumpForce;
            }

            // Apply the velocity
            _rigidbody2D.velocity = velocity;

            if (projectileThrower != null)
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                projectileThrower.UpdateAim(mouseWorldPosition);
                if (Input.GetMouseButtonDown(0))
                {
                    projectileThrower.GetThrowDirection(out Vector2 direction);
                    projectileThrower.ThrowProjectile(transform.position, direction, 10f);
                }
            }

            if (_lookAtTransform!=null)
            {
                Vector3 direction = _lookAtTransform.position - transform.position;
                if (direction.sqrMagnitude > 0.01f) // Minimum distance to avoid jitter
                {
                    direction.z = 0; // Ignore z-axis for 2D
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    transform.rotation = rotation;
                }
            }
        }

        public override void SetupComponent()
        {
        }

        public void LookAt(Transform transform1)
        {
            _lookAtTransform = transform1;
        }
    }
}
