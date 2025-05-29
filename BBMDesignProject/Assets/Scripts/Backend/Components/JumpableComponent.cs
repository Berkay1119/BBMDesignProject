using Backend.Attributes;

namespace Backend.Components
{
    [Component]
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