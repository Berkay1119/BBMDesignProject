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

        private void Update()
        {
            if (!_characterComponent)
            {
                return;
            }
            
            if (Input.GetKey(GetKeyCode(_characterComponent.LeftKey)))
            {
                var force = _characterComponent.Speed * Time.deltaTime;
                _rigidbody2D.AddForce(Vector2.left * force,ForceMode2D.Impulse);
            }
            else if (Input.GetKey(GetKeyCode(_characterComponent.RightKey)))
            {
                var force = _characterComponent.Speed * Time.deltaTime;
                _rigidbody2D.AddForce(Vector2.right * force,ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.AddForce(Vector2.up * _characterComponent.JumpForce, ForceMode2D.Impulse);
            }
        }
    }
}