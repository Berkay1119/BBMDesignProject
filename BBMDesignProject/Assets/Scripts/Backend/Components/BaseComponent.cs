using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend.Components
{
    public abstract class BaseComponent:MonoBehaviour
    {
        private string _name;
        private string _description;
        public string Name => _name;
        public string Description => _description;

        protected  List<Component> _addedComponents = new List<Component>();
        public void SetName(string name)
        {
            _name = name;
        }
        
        public void SetDescription(string description)
        {
            _description = description;
        }

        private void OnDisable()
        {
            foreach (var component in _addedComponents)
            {
                Destroy(component);
            }
        }

        public virtual void DrawGUI()
        {
            
        }

        public abstract void SetupComponent();
    }
}