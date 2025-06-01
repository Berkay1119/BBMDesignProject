using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Camera Follow Component")]
    public class CameraFollowComponent : BaseComponent, IUpdatable
    {
        public float smoothSpeed = 1.0f;
        public Vector3 offset;
        
        public override void SetupComponent()
        {
            
        }
        
        public CameraFollowComponent()
        {
            SetName("Camera Follow");
            SetDescription("This component makes the camera follow this object smoothly.");
        }
        
        private void Start() {
            offset = Camera.main.transform.position - this.transform.position;
        }
    
        public void OnUpdate()
        {
            Vector3 desiredPosition = transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, smoothSpeed);
            Camera.main.transform.position = smoothedPosition;
        }
    }
}