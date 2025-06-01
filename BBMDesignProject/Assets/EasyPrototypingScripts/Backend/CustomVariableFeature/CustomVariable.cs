namespace Backend.CustomVariableFeature
{
    public class CustomVariable
    {
        public VariableType Type;
        public string Name;
        public object Value;

        public CustomVariable(string newVariableName, VariableType newVariableType, string newVariableValue)
        {
            Name = newVariableName;
            Type = newVariableType;
            ParseTheValue(newVariableValue);
        }

        private void ParseTheValue(string newVariableValue)
        {
            if (newVariableValue == null)
            {
                return;
            }
            newVariableValue = newVariableValue.Trim();
            newVariableValue = newVariableValue.Replace(" ", "");
            newVariableValue = newVariableValue.ToLower();
            switch (Type)
            {
                case VariableType.String:
                    Value = newVariableValue;
                    break;
                case VariableType.Integer:
                    Value = int.Parse(newVariableValue);
                    break;
                case VariableType.Float:
                    Value = float.Parse(newVariableValue);
                    break;
                case VariableType.Boolean:
                    Value = bool.Parse(newVariableValue);
                    break;
            }
        }
    }
}