using System.Collections.Generic;
using Backend.Interfaces;
using UnityEngine;

namespace Backend.Managers
{
    public class UpdateManager : MonoBehaviour
    {
        private static UpdateManager _instance;
        private static bool _quitting;
        
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
        
        public static UpdateManager Instance
        {
            get
            {
                // If we’re in the middle of quitting, just return null
                if (_quitting)
                {
                    return null;
                }

                if (_instance == null)
                {
                    Debug.LogError("UpdateManager instance not found! (It may have been destroyed or not initialized)");
                }
                
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                // Prevent this object from being destroyed on scene loads
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                // If a new instance is found, log a warning and destroy it
                Debug.LogWarning("Another instance of UpdateManager was created. Destroying the duplicate instance.");
                Destroy(gameObject);
            }
        }
        
        private void OnApplicationQuit()
        {
            Shutdown();   
            _quitting = true;
        }
        
        private void OnDestroy()
        {
            // Clear the static reference on destroy
            if (_instance == this)
                _instance = null;
        }
        
        /// <summary>
        /// Clears all records, destroys singleton instance and game object
        /// </summary>
        private void Shutdown()
        {
            Debug.Log("Shutting down UpdateManager");
            _updatables.Clear();
            _fixedUpdatables.Clear();
            _instance = null;
            Destroy(gameObject);
        }
        
        public void Register(IUpdatable updatable)
        {
            RegisterHelper(_updatables, updatable, "updateable");
        }

        public void Register(IFixedUpdatable fixedUpdatable)
        {
            RegisterHelper(_fixedUpdatables, fixedUpdatable, "fixed updateable");
        }

        private void Update()
        {
            foreach (var updateable in _updatables)
            {
                updateable.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var fixedUpdateable in _fixedUpdatables)
            {
                fixedUpdateable.OnFixedUpdate();
            }
        }
        
        private static void RegisterHelper<T>(List<T> list, T item, string label)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                Debug.Log($"Registered {label}: {item}");
            }
        }
        
    }
}