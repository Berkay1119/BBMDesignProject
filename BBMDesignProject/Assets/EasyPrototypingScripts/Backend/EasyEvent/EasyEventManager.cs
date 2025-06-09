using System;
using System.Collections.Generic;
using Backend.EasyEvent.Actions;
using UnityEngine;

namespace Backend.EasyEvent
{
    public class EasyEventManager : MonoBehaviour
    {
        [SerializeField] private List<EasyEvent> events = new List<EasyEvent>();
        public List<EasyEvent> Events => events;

        private void OnEnable()
        {
            foreach (var easyEvent in events)
            {
                easyEvent.Setup();
            }
        }

        public void AddEvent(EasyEvent easyEvent)
         {
              events.Add(easyEvent);
              easyEvent.Setup(); 
         }

        private void OnValidate()
        {
            foreach (var easyEvent in events)
            {
                foreach (var action in easyEvent.Actions)
                {
                    if (action is InvokeAction)
                    {
                        action.actionName = "Invoke Event";
                        action.actionDescription = "This Event invokes the UnityEvent when executed.";
                    }
                }
            }
        }


        public void RemoveEvent(int i)
         {
                events.RemoveAt(i);
         }

         public void SaveEvent(int i, EasyEvent newEvent)
         {
                events[i].Conditions = newEvent.Conditions;
                events[i].Actions = newEvent.Actions;
                events[i].eventName = newEvent.eventName;
                events[i].eventDescription = newEvent.eventDescription;
                events[i].Setup(); 
         }

         public void SaveAllEvents(List<EasyEvent> newEvents)
         {
             if (newEvents == null || newEvents.Count != events.Count)
             {
                 Debug.LogError("Cannot save events: Mismatched event count.");
                 return;
             }

             for (int i = 0; i < newEvents.Count; i++)
             {
                 SaveEvent(i, newEvents[i]);
             }
         }

         public void ClearAllEvents()
         {
             
                events.Clear();
         }
    }
}