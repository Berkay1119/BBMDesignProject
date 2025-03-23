using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class EasyEventManager:MonoBehaviour
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
         }


         public void RemoveEvent(int i)
         {
                events.RemoveAt(i);
         }

         public void SaveEvent(int i, EasyEvent newEvent)
         {
                events[i].Condition = newEvent.Condition;
                events[i].Action = newEvent.Action;
                events[i].eventName = newEvent.eventName;
                events[i].eventDescription = newEvent.eventDescription;
         }

         private void Update()
         {
             foreach (var easyEvent in events)
             {
                 easyEvent.CheckCondition();
             }
         }
    }
}