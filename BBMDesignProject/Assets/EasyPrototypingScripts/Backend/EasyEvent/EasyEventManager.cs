using System.Collections.Generic;
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
         
    }
}