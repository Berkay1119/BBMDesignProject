using System.Collections.Generic;
using Backend.EasyEvent;
using Backend.Interfaces;
using Backend.Managers;
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
        
        private void Awake()
        {
            EventBus.PublishSpawn(this);
        }
        
        public void SetName(string name)
        {
            _name = name;
        }
        
        public void SetDescription(string description)
        {
            _description = description;
        }

        protected virtual void OnEnable()
        {
            switch (this)
            {
                case IUpdatable updateable:
                    UpdateManager.Instance.Register(updateable);
                    break;
                case IFixedUpdatable fixedUpdateable:
                    UpdateManager.Instance.Register(fixedUpdateable);
                    break;
            }
        }

        protected virtual void OnDisable()
        {
            switch (this)
            {
                case IUpdatable updateable:
                    UpdateManager.Instance.Unregister(updateable);
                    break;
                case IFixedUpdatable fixedUpdateable:
                    UpdateManager.Instance.Unregister(fixedUpdateable);
                    break;
            }

            foreach (var component in _addedComponents)
            {
                Destroy(component);
            }
            
            EventBus.PublishDestroy(this);
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            var other = col.gameObject.GetComponent<BaseComponent>();
            if (other != null)
                EventBus.PublishCollision(this, other);
        }

        public virtual void DrawGUI()
        {
            
        }

        public abstract void SetupComponent();
    }
}