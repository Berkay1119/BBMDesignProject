using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class AvatarComponent: BaseComponent
    {
        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody2D;
        public AvatarComponent()
        {
            SetName("Avatar");
            SetDescription("This component represents an avatar in the game, typically used for player characters.");
        }
        
        public override void SetupComponent()
        {
            gameObject.tag = "Avatar";
            gameObject.layer = LayerMask.NameToLayer("Avatar");
        }
        
        protected override void OnEnable() {
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }
            
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

            if (_boxCollider2D == null)
            {
                _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            }
        }
        
    }
}