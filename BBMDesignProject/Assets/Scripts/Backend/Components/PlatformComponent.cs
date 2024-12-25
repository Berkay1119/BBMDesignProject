using Backend.Attributes;
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
        [SerializeField] public bool IsMoving = false;
        [SerializeField] public MovingDirection Direction = MovingDirection.Horizontal;

        [SerializeField] [Range(-10f, 10f)] public float minX = -2f;
        [SerializeField] [Range(-10f, 10f)] public float maxX = 2f;
        [SerializeField] [Range(-10f, 10f)] public float minY = -1f;
        [SerializeField] [Range(-10f, 10f)] public float maxY = 1f;

        [SerializeField] public float speed = 1f;

        private Rigidbody2D _rigidbody2D;
        private Vector3 _startPosition;
        private float _directionMultiplier = 1f;

        private void Awake()
        {
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic; // Platform hareketi için gerekli
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        public PlatformComponent()
        {
            SetName("Platform");
            SetDescription("A platform that the player can stand on.");
        }

        private void FixedUpdate()
        {
            if (!IsMoving) return;

            Vector2 velocity = Vector2.zero;

            if (Direction == MovingDirection.Horizontal)
            {
                if (Mathf.Abs(_startPosition.x - _rigidbody2D.position.x) >= maxX || Mathf.Abs(_rigidbody2D.position.x) <= minX)
                {
                    _directionMultiplier *= -1;
                }
                velocity.x = speed * _directionMultiplier;
            }
            else if (Direction == MovingDirection.Vertical)
            {
                if (Mathf.Abs(_startPosition.y - _rigidbody2D.position.y) >= maxY || Mathf.Abs(_rigidbody2D.position.y) <= minY)
                {
                    _directionMultiplier *= -1;
                }
                velocity.y = speed * _directionMultiplier;
            }

            _rigidbody2D.velocity = velocity;
        }


        private void OnValidate()
        {
            // Clamp values to ensure proper settings
            if (Direction == MovingDirection.Horizontal)
            {
                minY = maxY = 0f; // Reset Y values
            }
            else if (Direction == MovingDirection.Vertical)
            {
                minX = maxX = 0f; // Reset X values
            }

            maxX = Mathf.Max(minX, maxX);
            maxY = Mathf.Max(minY, maxY);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsMoving & collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(transform); // Oyuncuyu platforma bağla
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (IsMoving & collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(null); // Oyuncuyu platformdan ayır
            }
        }

        private void OnEnable()
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

    [CustomEditor(typeof(PlatformComponent))]
    public class PlatformComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Reference to the target object
            PlatformComponent platformComponent = (PlatformComponent)target;

            EditorGUILayout.LabelField("Platform Settings", EditorStyles.boldLabel);
            platformComponent.IsMoving = EditorGUILayout.Toggle("Is Moving", platformComponent.IsMoving);

            using (new EditorGUI.DisabledScope(!platformComponent.IsMoving))
            {
                platformComponent.Direction = (MovingDirection)EditorGUILayout.EnumPopup("Direction", platformComponent.Direction);

                if (platformComponent.Direction == MovingDirection.Horizontal)
                {
                    platformComponent.minX = EditorGUILayout.FloatField("Min X", platformComponent.minX);
                    platformComponent.maxX = EditorGUILayout.FloatField("Max X", platformComponent.maxX);
                }
                else if (platformComponent.Direction == MovingDirection.Vertical)
                {
                    platformComponent.minY = EditorGUILayout.FloatField("Min Y", platformComponent.minY);
                    platformComponent.maxY = EditorGUILayout.FloatField("Max Y", platformComponent.maxY);
                }

                platformComponent.speed = EditorGUILayout.FloatField("Speed", platformComponent.speed);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(platformComponent);
            }
        }
    }
}
