using System.Collections.Generic;
using UnityEngine;

namespace Backend
{
    public class EasyEventManager:MonoBehaviour
    {
        private List<EasyEvent> events = new List<EasyEvent>();
       
         public void AddEvent(EasyEvent easyEvent)
         {
              events.Add(easyEvent);
             
         }
         
         
    }
}