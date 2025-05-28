using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
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
            }
            
            _boxCollider2D  = gameObject.GetComponent<BoxCollider2D>();
            if (_boxCollider2D == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
                //_addedComponents.Add(tempCollider);    destroy etmek için
            }
        }

 
    }
}
