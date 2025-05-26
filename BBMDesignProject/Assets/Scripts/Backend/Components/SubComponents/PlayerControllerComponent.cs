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
        
        public void Setup(CharacterComponent characterComponent, Rigidbody2D rigidbody2D)
        {
            _characterComponent = characterComponent;
            _rigidbody2D = rigidbody2D;
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
            
            var inputVel = 0f;
            if (Input.GetKey(GetKeyCode(_characterComponent.LeftKey)))
                inputVel = -_characterComponent.Speed;
            else if (Input.GetKey(GetKeyCode(_characterComponent.RightKey)))
                inputVel = _characterComponent.Speed;

            // Platform speed
            var platformVelX = _currentPlatformRb != null
                ? _currentPlatformRb.velocity.x
                : 0f;

            // Vertical speed
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.x = inputVel + platformVelX;

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
        }

        public override void SetupComponent()
        {
        }
    }
}
