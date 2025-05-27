using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Components
{
    [Component]
    public class InputActionComponent:BaseComponent, IUpdatable
    {
        [SerializeField] private KeyCode actionKey = KeyCode.Mouse0;
        [SerializeField] private UnityEvent actionEvent;
        public override void SetupComponent()
        {
            
        }
        
        public InputActionComponent()
        {
            SetName("Input Action");
            SetDescription("This component allows the object to respond to input actions.");
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(actionKey))
            {
                actionEvent?.Invoke();
            }
        }
    }
}