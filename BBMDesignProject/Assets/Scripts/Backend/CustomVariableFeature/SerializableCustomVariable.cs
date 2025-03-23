using System;
using UnityEngine;

namespace Backend.CustomVariableFeature
{
    public class SerializableCustomVariable:MonoBehaviour
    {
        [SerializeField] public string _name;
        [SerializeField] public string _value;
        [SerializeField] public VariableType _type;
        private CustomVariable _variable;
        public string Name => _name;
        public VariableType Type => _type;

        public void SetVariable(CustomVariable variable)
        {
            _variable = variable;
            _name = variable.Name;
            _value = variable.Value.ToString();
            _type = variable.Type;
        }

        private void OnValidate()
        {
            if (_variable == null) return;
            _variable.Name = _name;
            _variable.Type = _type;
            _variable.Value = _value; 
        }
    }
}