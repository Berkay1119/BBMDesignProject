using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Collectible Component")]
    public class CollectibleComponent: BaseComponent
    {
        public bool isPhysicsActive = false;
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
                _rigidbody2D.bodyType = isPhysicsActive ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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