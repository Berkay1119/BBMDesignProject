using System.Collections.Generic;
using Backend.Components;
using UnityEngine;

namespace Backend.Object
{
    public class EasyObject: MonoBehaviour
    {

        public void CreateEasyComponents(Dictionary<string,bool> components)
        {
            foreach (var component in components)
            {
                if (component.Value)
                {
                    var componentType = ComponentFactory.CreateComponent(component.Key);
                    if (componentType != null)
                    {
                           var addedComponent=gameObject.AddComponent(componentType);
                           BaseComponent baseComponent = (BaseComponent) addedComponent;
                           baseComponent.SetupComponent();
                    }
                }
            }
        }
        
        public void AddSprite(Texture2D texture)
        {
            if (texture== null) return;
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}