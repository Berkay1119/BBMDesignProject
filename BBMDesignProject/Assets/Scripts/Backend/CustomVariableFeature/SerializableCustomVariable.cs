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

        public void Add(int value)
        {
            if (_type != VariableType.Integer) throw new InvalidOperationException("Variable type is not Integer");
            if (int.TryParse(_value, out int currentValue))
            {
                _value = (currentValue + value).ToString();
            }
            else
            {
                throw new FormatException("Value is not a valid integer");
            }
        }
        
        public void Add(float value)
        {
            if (_type != VariableType.Float) throw new InvalidOperationException("Variable type is not Float");
            if (float.TryParse(_value, out float currentValue))
            {
                _value = (currentValue + value).ToString();
            }
            else
            {
                throw new FormatException("Value is not a valid float");
            }
        }
        
        public void SetValue(string value)
        {
            if (_type == VariableType.String)
            {
                _value = value;
            }
            else
            {
                throw new InvalidOperationException("Variable type is not String");
            }
        }
        
        public void SetValue(int value)
        {
            if (_type == VariableType.Integer)
            {
                _value = value.ToString();
            }
            else
            {
                throw new InvalidOperationException("Variable type is not Integer");
            }
        }
        
        public void SetValue(float value)
        {
            if (_type == VariableType.Float)
            {
                _value = value.ToString();
            }
            else
            {
                throw new InvalidOperationException("Variable type is not Float");
            }
        }
    }
}