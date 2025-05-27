using Backend.Attributes;
using Backend.Components.SubComponents;
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

        protected override void OnEnable() {
            
        }

        public override void SetupComponent()
        {
            gameObject.tag = "Avatar";
            gameObject.layer = 7;
        }
    }
}