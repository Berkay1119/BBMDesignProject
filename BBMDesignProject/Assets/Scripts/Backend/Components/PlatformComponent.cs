using Backend.Attributes;
using Backend.Objects;

namespace Backend.Components
{
    [Component]
    public class PlatformComponent: BaseComponent
    {
        public PlatformComponent()
        {
            IsStatic = true;
            HasCollider = true;
            IsTrigger = false;
            SetName("Platform");
            SetDescription("A platform that the player can stand on.");
        }
    }
}