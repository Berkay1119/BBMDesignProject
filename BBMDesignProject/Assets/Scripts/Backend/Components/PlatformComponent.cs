using System.Collections.Generic;
using Backend.Attributes;
using Backend.Object;
using UnityEditor;
using UnityEngine;

namespace Backend.Components
{
    public enum MovingDirection
    {
        Horizontal,
        Vertical
    }
    
    [Component]
    public class PlatformComponent : BaseComponent
    {
        [SerializeField] public MovingDirection Direction = MovingDirection.Horizontal;
        
        [SerializeField] public bool IsMoving = false;

        [SerializeField] List<EasyObject> waypoints = new List<EasyObject>();

        [SerializeField] public float oneLoopDuration = 1f;

        private Rigidbody2D _rigidbody2D;
        private Vector3 _startPosition;
                
        private int currentWaypointIndex = 0;
        private float waypointTimer = 0f;
        
        private List<float> segmentLengths;
        private float totalPathLength;

        private void Awake()
        {
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic; // Platform hareketi için gerekli
        }

        public PlatformComponent()
        {
            SetName("Platform");
            SetDescription("A platform that the player can stand on.");
        }

        private void Update()
        {
            if (!IsMoving || waypoints.Count == 0) return;

            waypointTimer += Time.deltaTime;

            float currentSegmentLength = segmentLengths[currentWaypointIndex];
            float segmentDuration = (currentSegmentLength / totalPathLength) * oneLoopDuration;

            float t = waypointTimer / segmentDuration;

            Vector3 start = waypoints[currentWaypointIndex % waypoints.Count].transform.position;
            Vector3 end = waypoints[(currentWaypointIndex + 1) % waypoints.Count].transform.position;

            Vector3 newPosition = Vector3.Lerp(start, end, t);

            Vector2 velocity = (newPosition - transform.position) / Time.fixedDeltaTime;
            _rigidbody2D.velocity = velocity;

            if (t >= 1f)
            {
                waypointTimer = 0f;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
        }

        private void Start()
        {
            CalculateSegmentLengths();
        }

        private void CalculateSegmentLengths()
        {
            segmentLengths = new List<float>();
            totalPathLength = 0f;

            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 start = waypoints[i].transform.position;
                Vector3 end = waypoints[(i + 1) % waypoints.Count].transform.position;
                float segmentLength = Vector3.Distance(start, end);
                segmentLengths.Add(segmentLength);
                totalPathLength += segmentLength;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsMoving & collision.gameObject.CompareTag("Character"))
            {
                collision.transform.SetParent(transform); // Connect the character to the platform
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (IsMoving & collision.gameObject.CompareTag("Character"))
            {
                collision.transform.SetParent(null); // Split the character from the platform
            }
        }

        protected override void OnEnable()
        {
            if (gameObject.GetComponent<BoxCollider2D>() == null)
            {
                var tempCollider = gameObject.AddComponent<BoxCollider2D>();
                _addedComponents.Add(tempCollider);
            }
        }

        public override void SetupComponent()
        {
            // Custom setup if needed
        }
    }
}
