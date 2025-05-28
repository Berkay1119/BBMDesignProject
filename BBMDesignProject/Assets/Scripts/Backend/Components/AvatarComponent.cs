using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class AvatarComponent: BaseComponent
    {
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
            
        }
        
    }
}