using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Backend.Components
{
    [Component]
    public class TimerComponent:BaseComponent
    {
        [SerializeField] private int durationBySecond = 5;
        [SerializeField] private List<UnityEvent> onTimerEnd;
        
        [SerializeField] private bool isLooping = false;
        public override void SetupComponent()
        {
            
        }
        
        
        public TimerComponent()
        {
            SetName("Timer");
            SetDescription("This component allows the object to have a timer functionality.");
        }

        private void Start()
        {
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(durationBySecond);
            foreach (var unityEvent in onTimerEnd)
            {
                unityEvent.Invoke();
            }
            
            if (isLooping)
            {
                StartCoroutine(StartTimer());
            }
        }


    }
}