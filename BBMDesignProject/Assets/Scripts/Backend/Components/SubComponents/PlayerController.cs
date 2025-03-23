using UnityEngine;

namespace Backend.Components.SubComponents
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterComponent _characterComponent;
        private Rigidbody2D _rigidbody2D;

        // For tracking platform movement
        private Transform _currentPlatform;
        private Vector3 _lastPlatformPosition;

        public void Setup(CharacterComponent characterComponent, Rigidbody2D rigidbody2D)
        {
            _characterComponent = characterComponent;
            _rigidbody2D = rigidbody2D;
        }

        private void Update()
        {
            if (!_characterComponent) return;

            HandleMovementInput();
            HandlePlatformMovement();
        }

        private void HandleMovementInput()
        {
            Vector2 velocity = _rigidbody2D.velocity;

            if (Input.GetKey(GetKeyCode(_characterComponent.LeftKey)))
                velocity.x = -_characterComponent.Speed;
            else if (Input.GetKey(GetKeyCode(_characterComponent.RightKey)))
                velocity.x = _characterComponent.Speed;
            else
                velocity.x = 0;

            if (Input.GetKeyDown(KeyCode.Space) && 
                Mathf.Abs(_rigidbody2D.velocity.y - (_currentPlatform==null ? 0f : _currentPlatform.GetComponent<Rigidbody2D>().velocity.y )) < 0.01f)
            {
                velocity.y = _characterComponent.JumpForce;
                Debug.Log("Jump");
            }
                
            _rigidbody2D.velocity = velocity;
        }

        private void HandlePlatformMovement()
        {
            if (transform.parent != null && transform.parent != _currentPlatform)
            {
                // Just stepped onto a new platform
                _currentPlatform = transform.parent;
                _lastPlatformPosition = _currentPlatform.position;
            }

            if (_currentPlatform != null)
            {
                Vector3 platformDelta = _currentPlatform.position - _lastPlatformPosition;
                
                // Apply platform's positional movement directly to player
                if (platformDelta != Vector3.zero)
                    transform.position += platformDelta;

                _lastPlatformPosition = _currentPlatform.position;
            }
        }

        private KeyCode GetKeyCode(char character)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), character.ToString().ToUpper());
        }
    }
}
