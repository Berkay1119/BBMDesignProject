namespace Backend.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ComponentAttribute : System.Attribute
    {
        private string _name;
        private string _description;
        
        public ComponentAttribute(string name, string description)
        {
            _name = name;
            _description = description;
        }
    }
}