using System;
using UnityEngine;

namespace Backend.CustomVariableFeature
{
    public class SerializableCustomVariable:MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private string _value;
        [SerializeField] private VariableType _type;
        private CustomVariable _variable;

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