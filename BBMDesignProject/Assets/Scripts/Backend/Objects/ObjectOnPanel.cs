using System.Collections.Generic;
using System.Linq;
using Backend.Components;
using UnityEngine;

namespace Backend.Objects
{
    public class ObjectOnPanel
    {
        private readonly Texture2D _texture;
        private string _name;
        private List<BaseComponent> _components;
        public Vector2 Position { get; set; }
        public List<BaseComponent> Components => _components;
        
        public bool HasSprite => _texture != null;
        public string Name => _name;
        public float Scale { get; set; }

        public ObjectOnPanel(Texture2D texture, string name, Dictionary<string,bool> components)
        {
            _texture = texture;
            Scale = 1;
            _name = name;
            _components = new List<BaseComponent>();
            foreach (var component in components)
            {
                if (component.Value)
                {
                    _components.Add(ComponentFactory.CreateComponent(component.Key));
                }
            }
            Position = Vector2.zero;
        }
        
        public Texture2D GetTexture()
        {
            return _texture;
        }
    }
}