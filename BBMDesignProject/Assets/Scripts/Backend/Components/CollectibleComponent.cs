using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CollectibleComponent: BaseComponent
    {
        private BoxCollider2D _boxCollider2D;
        
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