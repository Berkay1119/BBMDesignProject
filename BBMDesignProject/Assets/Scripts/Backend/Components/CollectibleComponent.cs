using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CollectibleComponent: BaseComponent
    {
        [SerializeField] bool useGravity = false;
        [SerializeField] private float gravityScale = 1f;
        
        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody2D;
        
        public CollectibleComponent()
        {
            SetName("Collectible");
            SetDescription("This component makes the object collectible");
        }
        
        public override void SetupComponent()
        {
            gameObject.tag = "Collectible";
            gameObject.layer = LayerMask.NameToLayer("Collectible");
        }
        
        private void Awake()
        {
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }
            _rigidbody2D.gravityScale = gravityScale;
            
            if (!useGravity)
            {
                // Stays in the air
                _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            }
            
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

            if (_boxCollider2D == null)
            {
                _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            GUILayout.BeginVertical("box");
            GUILayout.Label("Collectible Settings", EditorStyles.boldLabel);
            GUILayout.EndVertical();
        }
    }
    
}