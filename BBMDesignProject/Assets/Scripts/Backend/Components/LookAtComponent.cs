using Backend.Attributes;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Components
{
    public enum FacingDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    [Component]
    [AddComponentMenu("EasyPrototyping/Look At Component")]
    public class LookAtComponent : BaseComponent, IUpdatable
    {
        [SerializeField] private GameObject target;
        
        [SerializeField] private FacingDirection defaultFacing = FacingDirection.Up;

        public override void SetupComponent() { }

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
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Apply offset based on default sprite facing direction
                    angle += GetRotationOffset();

                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }

        private float GetRotationOffset()
        {
            switch (defaultFacing)
            {
                case FacingDirection.Right: return 0f;
                case FacingDirection.Up: return -90f;
                case FacingDirection.Left: return 180f;
                case FacingDirection.Down: return 90f;
                default: return 0f;
            }
        }
    }
}