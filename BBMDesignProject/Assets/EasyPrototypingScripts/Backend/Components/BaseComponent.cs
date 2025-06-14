﻿using System.Collections.Generic;
using Backend.EasyEvent;
using Backend.Interfaces;
using Backend.Managers;
using UnityEngine;

namespace Backend.Components
{
    public abstract class BaseComponent : MonoBehaviour
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
        
        protected void SetName(string name)
        {
            _name = name;
        }
        
        protected void SetDescription(string description)
        {
            _description = description;
        }

        protected virtual void OnEnable()
        {
            if (this is IUpdatable u)
            {
                UpdateManager.Instance?.Register(u);
            }

            if (this is IFixedUpdatable f)
            {
                UpdateManager.Instance?.Register(f);
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var component in _addedComponents)
            {
                Destroy(component);
            }
            
            if (this is IUpdatable u)
            {
                UpdateManager.Instance?.Unregister(u);
            }

            if (this is IFixedUpdatable f)
            {
                UpdateManager.Instance?.Unregister(f);
            }
            
            EventBus.PublishDestroy(this);
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
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