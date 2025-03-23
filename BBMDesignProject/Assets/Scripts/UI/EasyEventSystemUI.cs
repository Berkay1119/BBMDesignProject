using System.Collections.Generic;
using System.Linq;
using Backend;
using Backend.Attributes;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class EasyEventSystemUI : EditorWindow
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

                // Event details
                _events[i].eventName = EditorGUILayout.TextField("Event Name", _events[i].eventName);
                _events[i].eventDescription = EditorGUILayout.TextField("Event Description", _events[i].eventDescription);

                var conditions = AttributeFinder.FindClassesWithAttribute<ConditionAttribute>();
                var actions = AttributeFinder.FindClassesWithAttribute<ActionAttribute>();

                // Dropdown for adding conditions
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Condition"), FocusType.Keyboard))
                {
                    var conditionMenu = new GenericMenu();
                    foreach (var condition in conditions)
                    {
                        var i1 = i;
                        conditionMenu.AddItem(new GUIContent(condition.Name), false, () =>
                        {
                            var classInstance = (EasyCondition)System.Activator.CreateInstance(condition);
                            _events[i1].Conditions.Add(classInstance);
                        });
                    }
                    conditionMenu.ShowAsContext();
                }

                // Display selected conditions
                GUILayout.Label("Conditions:", EditorStyles.boldLabel);
                for (int c = 0; c < _events[i].Conditions.Count; c++)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(_events[i].Conditions[c].conditionName);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        _events[i].Conditions.RemoveAt(c);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    _events[i].Conditions[c].DrawGUI();
                }

                // Dropdown for adding actions
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Action"), FocusType.Keyboard))
                {
                    var actionMenu = new GenericMenu();
                    foreach (var action in actions)
                    {
                        var i1 = i;
                        actionMenu.AddItem(new GUIContent(action.Name), false, () =>
                        {
                            var classInstance = (EasyAction)System.Activator.CreateInstance(action);
                            _events[i1].Actions.Add(classInstance);
                        });
                    }
                    actionMenu.ShowAsContext();
                }

                // Display selected actions
                GUILayout.Label("Actions:", EditorStyles.boldLabel);
                for (int a = 0; a < _events[i].Actions.Count; a++)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(_events[i].Actions[a].actionName);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        _events[i].Actions.RemoveAt(a);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    _events[i].Actions[a].DrawGUI();
                }

                // Save button
                if (GUILayout.Button("Save Event"))
                {
                    GetEasyEventManager().SaveEvent(i, _events[i]);
                }

                // Remove event
                if (GUILayout.Button("Remove Event"))
                {
                    _events.RemoveAt(i);
                    GetEasyEventManager().RemoveEvent(i);
                    break;
                }
                GUILayout.EndVertical();
            }

            // Add new event
            if (GUILayout.Button("Add New Event"))
            {
                var newEvent = new EasyEvent();
                newEvent.Conditions = new List<EasyCondition>();
                newEvent.Actions = new List<EasyAction>();

                GetEasyEventManager().AddEvent(newEvent);
                _events.Add(newEvent);
            }
            GUILayout.EndVertical();
        }

        private EasyEventManager GetEasyEventManager()
        {
            if (_easyEventManager == null)
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
