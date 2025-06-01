using System.Collections.Generic;
using Backend.Attributes;
using Backend.Object;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Waypoint Move Component")]
    public class WaypointMoveComponent : BaseComponent
    {
        [SerializeField] private List<EasyObject> waypoints = new List<EasyObject>();
        [SerializeField] private float oneLoopDuration = 1f;
        [SerializeField] private Color color = Color.green;

        private int index = 0;
        private float timer = 0f;
        private List<float> segmentLengths;
        private float totalPathLength;
        private Rigidbody2D _rigidbody2D;
        
        public WaypointMoveComponent()
        {
            SetName("Waypoint Movement");
            SetDescription("Moves the object between defined waypoints based on total loop duration.");
        }

        public override void SetupComponent()
        {
            
        }

        private void Awake()
        {
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            }

            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody2D.gravityScale = 0f; // Disable gravity for waypoint movement
            CalculateSegmentLengths();
        }

        private void Update()
        {
            if (waypoints.Count == 0 || totalPathLength == 0f) return;
            
            float currentSegmentLength = segmentLengths[index];
            float segmentDuration = (currentSegmentLength / totalPathLength) * oneLoopDuration;
            timer += Time.deltaTime;
            float t = timer / segmentDuration;

            Vector3 start = waypoints[index % waypoints.Count].transform.position;
            Vector3 end = waypoints[(index + 1) % waypoints.Count].transform.position;

            Vector3 newPosition = Vector3.Lerp(start, end, t);
            Vector2 velocity = (newPosition - transform.position) / Time.fixedDeltaTime;
            _rigidbody2D.velocity = velocity;

            if (t >= 1f)
            {
                timer = 0f;
                index = (index + 1) % waypoints.Count;
            }
        }

        private void CalculateSegmentLengths()
        {
            segmentLengths = new List<float>();
            totalPathLength = 0f;

            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 start = waypoints[i].transform.position;
                Vector3 end = waypoints[(i + 1) % waypoints.Count].transform.position;
                
                float distance = Vector3.Distance(start, end);
                segmentLengths.Add(distance);
                totalPathLength += distance;
            }
        }

        private  void OnDrawGizmos()
        {
            if (waypoints.Count < 2) return;

            Gizmos.color = color;
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (waypoints[i] == null)
                {
                    continue;
                }
                Vector3 start = waypoints[i].transform.position;
                Vector3 end = waypoints[(i + 1) % waypoints.Count].transform.position;
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(start, 0.1f);
            }
        }
        
    }
}
