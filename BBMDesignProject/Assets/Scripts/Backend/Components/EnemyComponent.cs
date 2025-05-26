using System;
using System.Collections.Generic;
using Backend.Attributes;
using Backend.Object;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class EnemyComponent:BaseComponent
    {
        [SerializeField] List<EasyObject> waypoints = new List<EasyObject>();
        
        [SerializeField] public float oneLoopDuration = 1f;
        
        [SerializeField] private bool navMeshAgentEnabled = false;
        
        [SerializeField] private Transform navMeshTarget;
        
        private int currentWaypointIndex = 0;
        private float waypointTimer = 0f;
        
        private List<float> segmentLengths;
        private float totalPathLength;
        
        private Rigidbody2D _rigidbody2D;
        public override void SetupComponent()
        {
            
        }
        
        public EnemyComponent()
        {
            SetName("Enemy");
            SetDescription("An enemy that moves between waypoints.");
        }

        private void Awake()
        {
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<BoxCollider2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        private void Update()
        {
            waypointTimer += Time.deltaTime;

            if (waypoints.Count==0)
            {
                return;
            }
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
        
    }
}