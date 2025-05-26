using Backend.EasyEvent.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Components
{
    public class CustomEventListenerComponent:BaseComponent
    {
        
        [SerializeField] private CustomEventScriptableObject gameEvent;
        
        [SerializeField] private UnityEvent response;
        public override void SetupComponent()
        {
            
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