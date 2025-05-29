using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Solid Component")]
    public class SolidComponent : BaseComponent
    {
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;

        public SolidComponent()
        {
            SetName("Solid");
            SetDescription("A solid surface that the player can collide with or stand on.");
        }
        
        public override void SetupComponent()
        {
            gameObject.tag = "Solid";
            gameObject.layer = LayerMask.NameToLayer("Solid");
        }
        
        
        protected override void OnEnable()
        {
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody2D.bodyType = RigidbodyType2D.Static; // Solid objects are typically static
            }
            
            _boxCollider2D  = gameObject.GetComponent<BoxCollider2D>();
            if (_boxCollider2D == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }

 
    }
}
