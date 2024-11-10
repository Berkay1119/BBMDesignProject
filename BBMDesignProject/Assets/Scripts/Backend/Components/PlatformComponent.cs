using Backend.Attributes;
using Backend.Objects;

namespace Backend.Components
{
    [Component("Platform", "A platform that the player can stand on.")]
    public class PlatformComponent: BaseComponent
    {
        public PlatformComponent()
        {
            IsStatic = true;
            HasCollider = true;
            IsTrigger = false;
        }
    }
}