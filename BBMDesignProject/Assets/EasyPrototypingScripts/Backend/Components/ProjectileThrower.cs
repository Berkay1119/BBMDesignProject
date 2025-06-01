using Backend.Attributes;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Projectile Thrower Component")]
    public class ProjectileThrower : BaseComponent
    {
        [SerializeField] private ProjectileComponent projectilePrefab;
        [SerializeField] private GameObject aimGameObject;
        
        public ProjectileThrower()
        {
            SetName("Projectile Thrower");
            SetDescription("This component allows the object to throw projectiles.");
        }

        public override void SetupComponent()
        {
            
        }
        
        private void ThrowProjectile(Vector2 startPosition, Vector2 direction, float speed)
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab is not assigned.");
                return;
            }
            
            Vector2 spawnPosition = startPosition + direction * 1f; 
            
            ProjectileComponent projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.Initialize(gameObject, spawnPosition, direction, speed);
        }
        
        
        private void GetThrowDirection(out Vector2 direction)
        {
            if (aimGameObject == null)
            {
                Debug.LogError("Aim GameObject is not assigned.");
                direction = Vector2.zero;
                return;
            }

            Vector2 aimPosition = aimGameObject.transform.position;
            Vector2 throwPosition = transform.position;
            direction = (aimPosition - throwPosition).normalized;

            if (direction == Vector2.zero)
            {
                direction = Vector2.right; // Return to default
            }
        }
        
        public void Shoot()
        {
            GetThrowDirection(out Vector2 direction); 
            ThrowProjectile(transform.position, direction, 10f);
        }
    }
}