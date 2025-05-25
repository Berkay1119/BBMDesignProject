using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CollectibleComponent: BaseComponent
    {
        
        public CollectibleComponent()
        {
            SetName("Collectible");
            SetDescription("This component makes the object collectible");
        }
        
        public override void SetupComponent()
        {
            gameObject.tag = "Collectible";
            gameObject.AddComponent<BoxCollider2D>();
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