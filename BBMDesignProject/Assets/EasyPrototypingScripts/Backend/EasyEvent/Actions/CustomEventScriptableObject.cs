using System.Collections.Generic;
using Backend.Components;
using UnityEngine;

namespace Backend.EasyEvent.Actions
{
    [CreateAssetMenu(fileName = "CustomEvent", menuName = "EasyEvent/CustomEvent", order = 1)]
    public class CustomEventScriptableObject : ScriptableObject
    {
        private readonly List<CustomEventListenerComponent> listeners = new();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void RegisterListener(CustomEventListenerComponent listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(CustomEventListenerComponent listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}