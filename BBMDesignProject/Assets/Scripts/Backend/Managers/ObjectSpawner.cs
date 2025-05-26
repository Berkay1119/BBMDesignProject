using UnityEngine;

namespace Backend.Managers
{
    public abstract class ObjectSpawner<T> : MonoBehaviour
    {
        private T SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is null. Cannot spawn object.");
                return default(T);
            }

            GameObject spawnedObject = Instantiate(prefab, position, rotation);
            return spawnedObject.GetComponent<T>();
        }

        
    }
}