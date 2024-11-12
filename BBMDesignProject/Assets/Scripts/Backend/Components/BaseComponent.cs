namespace Backend.Components
{
    public abstract class BaseComponent
    {
        protected bool IsStatic;
        protected bool HasCollider;
        protected bool IsTrigger;
        private string _name;
        private string _description;
        public string Name => _name;
        public string Description => _description;

        
        public void SetName(string name)
        {
            _name = name;
        }
        
        public void SetDescription(string description)
        {
            _description = description;
        }
    }
}