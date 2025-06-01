using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Follow Mouse Component")]
    public class FollowMouseComponent:BaseComponent,IUpdatable
    {
        public override void SetupComponent()
        {
            
        }

        public FollowMouseComponent()
        {
            SetName("Follow Mouse");
            SetDescription("This component allows the object to follow the mouse cursor in 2D space.");
        }

        public void OnUpdate()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Ensure the z-coordinate is zero for 2D
            transform.position = mouseWorldPosition;
        }
    }
}