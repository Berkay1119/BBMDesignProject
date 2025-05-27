using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class LookAtComponent: BaseComponent, IUpdatable
    {
        [SerializeField] private GameObject target;
        public override void SetupComponent()
        {
            
        }
        
        public LookAtComponent()
        {
            SetName("Look At");
            SetDescription("This component allows the object to look at a specified target in 2D space.");
        }

        public void OnUpdate()
        {
            if (target != null)
            {
                Vector3 direction = target.transform.position - transform.position;
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    transform.rotation = rotation;
                }
            }
        }
    }
}