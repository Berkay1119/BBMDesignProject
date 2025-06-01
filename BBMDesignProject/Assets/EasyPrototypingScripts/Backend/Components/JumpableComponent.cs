using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Jumpable Component")]
    public class JumpableComponent : BaseComponent
    {
        public JumpableComponent()
        {
            SetName("Jumpable");
            SetDescription("This component makes the object jumpable");
        }
        
        public override void SetupComponent()
        {
            
        }
    }
}