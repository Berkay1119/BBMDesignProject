using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Backend.Components
{
    [Component]
    [AddComponentMenu("EasyPrototyping/Timer Component")]
    public class TimerComponent:BaseComponent
    {
        [SerializeField] private int durationBySecond = 5;
        [SerializeField] private List<UnityEvent> onOneLoopEnd;
        [SerializeField] private List<UnityEvent> onTimerStart;
        [SerializeField] private List<UnityEvent> onTimerEnd;
        
        [SerializeField] private bool startOnAwake = true;
        [Tooltip("This is the number of times the timer will loop. If set to -1, it will loop indefinitely.")]
        [SerializeField] private int loopCount = 1;
        
        private Coroutine coroutine;
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
            if (startOnAwake)
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            if (coroutine != null)
            {
                return;
            }
            
            foreach (var unityEvent in onTimerStart)
            {
                unityEvent.Invoke();
            }
            
            coroutine=StartCoroutine(StartTimerCoroutine());
        }

        private IEnumerator StartTimerCoroutine()
        {
            for (int i = 0; i < loopCount; i++)
            {
                yield return new WaitForSeconds(durationBySecond);
                foreach (var unityEvent in onOneLoopEnd)
                {
                    unityEvent.Invoke();
                }
            }

            if (loopCount==-1)
            {
                yield return new WaitForSeconds(durationBySecond);
                foreach (var unityEvent in onOneLoopEnd)
                {
                    unityEvent.Invoke();
                }
            }
            
            foreach (var unityEvent in onTimerEnd)
            {
                unityEvent.Invoke();
            }
        }

        public void StopTimer()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

    }
}