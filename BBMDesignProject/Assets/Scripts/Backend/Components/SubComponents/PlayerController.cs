using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend.Components.SubComponents
{
    public class PlayerController:MonoBehaviour
    {
        private CharacterComponent _characterComponent;
        private Rigidbody2D _rigidbody2D;
        
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

        private void Update() {
            if (!_characterComponent) {
                return;
            }

            Vector2 velocity = _rigidbody2D.velocity;

            // Handle input for movement
            if (Input.GetKey(GetKeyCode(_characterComponent.LeftKey))) {
                velocity.x = -_characterComponent.Speed; 
            } else if (Input.GetKey(GetKeyCode(_characterComponent.RightKey))) {
                velocity.x = _characterComponent.Speed; 
            } else {
                velocity.x = 0;
            }

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(_rigidbody2D.velocity.y) < 0.01f) {
                velocity.y = _characterComponent.JumpForce;
            }

            if (transform.parent != null) {
                PlatformComponent platformComponent = transform.parent.GetComponent<PlatformComponent>();
                if (platformComponent != null) {
                    Rigidbody2D platformRigidbody = transform.parent.GetComponent<Rigidbody2D>();
                    if (platformRigidbody != null) {
                        // Adjust velocity based on platform's moving direction
                        switch (platformComponent.Direction) {
                            case MovingDirection.Horizontal:
                                velocity.x += platformRigidbody.velocity.x;
                                break;

                            case MovingDirection.Vertical:
                                velocity.y = platformRigidbody.velocity.y;
                                break;
                        }
                    }
                }
            }

            // Apply the calculated velocity to the character
            _rigidbody2D.velocity = velocity;
        }



    }
}