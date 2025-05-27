using Backend.Attributes;
using Backend.EasyEvent.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Components
{
    [Component]
    public class CustomEventListenerComponent:BaseComponent
    {
        [SerializeField] private CustomEventScriptableObject gameEvent;
        
        [SerializeField] private UnityEvent response;
        public override void SetupComponent()
        {
            
        }
        
        public CustomEventListenerComponent()
        {
            SetName("Custom Event Listener");
            SetDescription("This component listens for custom events and triggers a response when the event is raised.");
        }
        
        protected override void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        protected override void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response?.Invoke();
        }
    }
}