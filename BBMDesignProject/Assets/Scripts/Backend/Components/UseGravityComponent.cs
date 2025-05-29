using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Use Gravity Component")]
    public class UseGravityComponent : BaseComponent
    {
        [SerializeField] private float gravityScale = 1f;
        public UseGravityComponent()
        {
            SetName("Use Gravity");
            SetDescription("This component allows the object to use gravity in the game world.");
        }

        public override void SetupComponent()
        {
            // This component does not require any specific setup.
            // It simply enables the use of gravity for the object.
        }
        
        private void Start()
        {
            // Ensure the Rigidbody component is present and use gravity
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
            if (rb != null)
            {
                rb.gravityScale = gravityScale;
            }

            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}