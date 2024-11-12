using Backend.Attributes;

namespace Backend.Components
{
    [Component]
    public class CharacterComponent: BaseComponent
    {
        public CharacterComponent()
        {
            SetName("Character");
            SetDescription("This component is used to define a character in the game.");
            IsStatic = false;
            HasCollider = true;
            IsTrigger = false;
        }
    }
}