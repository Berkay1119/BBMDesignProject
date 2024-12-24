using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CollectibleComponent: BaseComponent
    {
        private CollectibleType _collectibleType;
        
        public CollectibleType CollectibleType => _collectibleType;
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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Collectible Type:", GUILayout.Width(150));
            _collectibleType = (CollectibleType)EditorGUILayout.EnumPopup(_collectibleType);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }

    public enum CollectibleType
    {
        Coin,
        Health,
        Ammo
    }
}