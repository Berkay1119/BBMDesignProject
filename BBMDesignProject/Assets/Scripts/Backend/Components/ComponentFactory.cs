namespace Backend.Components
{
    public class ComponentFactory
    {
        public static BaseComponent CreateComponent(string componentKey)
        {
            switch (componentKey)
            {
                case "Platform":
                    return new PlatformComponent();
                default:
                    return null;
            }
        }
    }
}