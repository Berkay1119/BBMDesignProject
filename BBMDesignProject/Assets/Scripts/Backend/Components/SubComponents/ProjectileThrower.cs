using System;
using System.Collections.Generic;
using Backend.Attributes;
using UnityEngine;

namespace Backend.Components.SubComponents
{
    [Component]
    public class ProjectileThrower : BaseComponent
    {
        [SerializeField] private ProjectileComponent projectilePrefab;
        [SerializeField] private GameObject aimGameObject;

        private void Start()
        {
            SetupComponent();
        }

        public override void SetupComponent()
        {
            if (GetComponent<PlayerControllerComponent>()!=null)
            {
                GetComponent<PlayerControllerComponent>().projectileThrower = this;
                aimGameObject.SetActive(true);
            }
            else
            {
                aimGameObject.SetActive(false);
            }
        }
        

        public void ThrowProjectile(Vector2 startPosition, Vector2 direction, float speed)
        {
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab is not assigned.");
                return;
            }
            
            Vector2 spawnPosition = startPosition + direction * 1f; // Yönün biraz önünde doğurmak için
            
            ProjectileComponent projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.Initialize(gameObject, spawnPosition, direction, speed);
        }

        public void UpdateAim(Vector3 worldPosition)
        {
            if (aimGameObject == null)
            {
                Debug.LogError("Aim GameObject is not assigned.");
                return;
            }

            aimGameObject.transform.position = new Vector3( worldPosition.x, worldPosition.y, aimGameObject.transform.position.z);
        }
        
        public void GetThrowDirection(out Vector2 direction)
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
                direction = Vector2.right; // Varsayılan yön
            }
        }
    }
}