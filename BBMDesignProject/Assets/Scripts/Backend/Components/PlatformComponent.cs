using System;
using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class PlatformComponent: BaseComponent
    {
        public PlatformComponent()
        {
            SetName("Platform");
            SetDescription("A platform that the player can stand on.");
        }

        private void OnEnable()
        {
            var tempCollider=gameObject.AddComponent<BoxCollider2D>();
            _addedComponents.Add(tempCollider);
        }

        public override void SetupComponent()
        {
            
        }
    }
}