using UnityEngine;

public class CameraFollowComponent : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 1.0f;
    public Vector3 offset;

    private void Start() {
        if (target != null) {
            offset = transform.position - target.position;
        }
        else {
            Destroy(this);
            Debug.LogWarning("Target is not assigned. Please assign a target for the camera to follow.");
        }
    }
    
    private void Update() {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}