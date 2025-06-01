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
        
        private Vector3 targetPos;
        
        public override void SetupComponent()
        {
            
        }
        
        public CameraFollowComponent()
        {
            SetName("Camera Follow");
            SetDescription("This component makes the camera follow this object smoothly.");
        }
    
        public void OnUpdate()
        {
            if (Camera.main == null)
            {
                Debug.Log("Main Camera is null!");
                return;
            }
            
            targetPos = transform.position;
            Camera.main.transform.position = 
                new Vector3(targetPos.x, targetPos.y, 0f) + 
                new Vector3(offset.x, offset.y, 0f) +
                new Vector3(0f, 0f, Camera.main.transform.position.z);
        }
    }
}