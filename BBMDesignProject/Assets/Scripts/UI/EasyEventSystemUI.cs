using System.Collections.Generic;
using System.Linq;
using Backend;
using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class EasyEventSystemUI:EditorWindow
    {
        private List<EasyEvent> _events = new List<EasyEvent>();
        private EasyEventManager _easyEventManager;
        [MenuItem("Window/Easy Event System")]
        public static void ShowWindow()
        {
            GetWindow<EasyEventSystemUI>("Easy Event System");
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Easy Event System", EditorStyles.boldLabel);
            _events = GetEasyEventManager().Events.ToList();
            for (int i = 0; i < _events.Count; i++)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label($"Event {i + 1}", EditorStyles.label);

                // Display event details
                _events[i].eventName = EditorGUILayout.TextField("Event Name", _events[i].eventName);
                _events[i].eventDescription = EditorGUILayout.TextField("Event Description", _events[i].eventDescription);
                var conditions = AttributeFinder.FindClassesWithAttribute<ConditionAttribute>();
                var actions = AttributeFinder.FindClassesWithAttribute<ActionAttribute>();
                
                var conditionDropdownMenu = new GenericMenu();
                var currentEvent = _events[i];
                foreach (var condition in conditions)
                {
                    conditionDropdownMenu.AddItem(new GUIContent($"Condition/{condition.Name}"), false, () =>
                    {
                        var classInstance = (EasyCondition)System.Activator.CreateInstance(condition);
                        currentEvent.Condition = classInstance;
                    });
                }
                var actionDropdownMenu = new GenericMenu();
                foreach (var action in actions)
                {
                    actionDropdownMenu.AddItem(new GUIContent($"Action/{action.Name}"), false, () =>
                    {
                        var classInstance = (EasyAction)System.Activator.CreateInstance(action);
                        currentEvent.Action = classInstance;
                    });
                }
                GUILayout.BeginHorizontal("box");
                if (EditorGUILayout.DropdownButton(new GUIContent(_events[i].Condition==null?"Select Condition": _events[i].Condition.conditionName), FocusType.Keyboard))
                {
                    conditionDropdownMenu.ShowAsContext();
                }
                if (EditorGUILayout.DropdownButton(new GUIContent( _events[i].Action==null ? "Select Action" : _events[i].Action.actionName), FocusType.Keyboard))
                {
                    actionDropdownMenu.ShowAsContext();
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal("box");
                if (_events[i].Condition != null)
                {
                    _events[i].Condition.DrawGUI();
                }
                GUILayout.Space(10);
                if (_events[i].Action != null)
                {
                    _events[i].Action.DrawGUI();
                }
                
                GUILayout.EndHorizontal();
                //Save button
                if (GUILayout.Button("Save Event"))
                {
                    GetEasyEventManager().SaveEvent(i, _events[i]);
                }
                
                // Remove button
                if (GUILayout.Button("Remove Event"))
                {
                    _events.RemoveAt(i);
                    GetEasyEventManager().RemoveEvent(i);
                    break;
                }
                GUILayout.EndVertical();
            }

            // Add new event button
            if (GUILayout.Button("Add New Event"))
            {
                GetEasyEventManager().AddEvent(new EasyEvent());
                _events.Add(new EasyEvent());
            }
            GUILayout.EndVertical();
        }
        
        private EasyEventManager GetEasyEventManager()
        {
            if (_easyEventManager== null)
            {
                _easyEventManager = FindObjectOfType<EasyEventManager>();
                if (_easyEventManager == null)
                {
                    _easyEventManager = new GameObject("EasyEventManager").AddComponent<EasyEventManager>();
                }
            }
            return _easyEventManager;
        }
    }
}