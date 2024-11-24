using System.Collections.Generic;
using Backend;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class EasyEventSystemUI:EditorWindow
    {
        private List<EasyEvent> _events = new List<EasyEvent>();
        [MenuItem("Window/Easy Event System")]
        public static void ShowWindow()
        {
            GetWindow<EasyEventSystemUI>("Easy Event System");
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Easy Event System", EditorStyles.boldLabel);
            for (int i = 0; i < _events.Count; i++)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label($"Event {i + 1}", EditorStyles.label);

                // Display event details
                _events[i].eventName = EditorGUILayout.TextField("Event Name", _events[i].eventName);
                _events[i].eventDescription = EditorGUILayout.TextField("Event Description", _events[i].eventDescription);
                
                // Remove button
                if (GUILayout.Button("Remove Event"))
                {
                    _events.RemoveAt(i);
                    break;
                }
                GUILayout.EndVertical();
            }

            // Add new event button
            if (GUILayout.Button("Add New Event"))
            {
                _events.Add(new EasyEvent());
            }
            GUILayout.EndVertical();
        
        }
    }
}