using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Attributes;
using Backend.EasyEvent;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class EasyEventSystemUI : EditorWindow
    {
        private List<EasyEvent> _events = new List<EasyEvent>();
        private EasyEventManager _easyEventManager;

        [MenuItem("Window/Easy Event System")]
        public static void ShowWindow() => GetWindow<EasyEventSystemUI>("Easy Event System");
        private Vector2 _scrollPos;
   
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Easy Event System", EditorStyles.boldLabel);

            var manager = GetEasyEventManager();
            
            // Begin scroll view
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));
            
            if (manager == null || manager.Events == null)
            {
                EditorGUILayout.HelpBox("EasyEventManager can not be found or there is no events.", MessageType.Warning);
                GUILayout.EndVertical();
                return;
            }
            
            _events = manager.Events.ToList();
            foreach (var ev in _events)
            {
                ev.Setup();
                if (ev.Conditions == null) ev.Conditions = new List<EasyCondition>();
                if (ev.Actions    == null) ev.Actions    = new List<EasyAction>();
            }

            int removeIndex = -1;

            var conditionTypes = AttributeFinder.FindClassesWithAttribute<ConditionAttribute>().ToArray();
            var actionTypes    = AttributeFinder.FindClassesWithAttribute<ActionAttribute>().ToArray();

            for (int i = 0; i < _events.Count; i++)
            {
                var ev = _events[i];
                if (ev == null) continue;

                var oldBg = GUI.backgroundColor;
                GUI.backgroundColor = new Color(1f, 0f, 0.54f, 1f);
                GUILayout.BeginVertical("box");
                GUI.backgroundColor = oldBg;
                
                ev.eventName        = EditorGUILayout.TextField("Event Name",        ev.eventName ?? "");
                ev.eventDescription = EditorGUILayout.TextField("Event Description", ev.eventDescription ?? "");

                // Add Condition
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Condition"), FocusType.Keyboard))
                {
                    var menu = new GenericMenu();
                    foreach (var t in conditionTypes)
                    {
                        menu.AddItem(new GUIContent(t.Name), false, () =>
                        {
                            var inst = (EasyCondition) Activator.CreateInstance(t);
                            inst.Setup(ev);            
                            ev.Conditions.Add(inst);
                        });
                    }
                    menu.ShowAsContext();
                }

                // Show Conditions
                EditorGUILayout.LabelField("Conditions:", EditorStyles.boldLabel);
                for (int c = 0; c < ev.Conditions.Count; c++)
                {
                    var cond = ev.Conditions[c];
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(cond.conditionName ?? cond.GetType().Name);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        ev.Conditions.RemoveAt(c);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    cond.DrawGUI();
                }

                // Add Action
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Action"), FocusType.Keyboard))
                {
                    var menu = new GenericMenu();
                    foreach (var t in actionTypes)
                    {
                        menu.AddItem(new GUIContent(t.Name), false, () =>
                        {
                            ev.Actions.Add((EasyAction)Activator.CreateInstance(t));
                        });
                    }
                    menu.ShowAsContext();
                }

                // Show Actions
                EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);
                for (int a = 0; a < ev.Actions.Count; a++)
                {
                    var act = ev.Actions[a];
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(act.actionName ?? act.GetType().Name);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        ev.Actions.RemoveAt(a);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    act.DrawGUI();
                }

                // Save and Remove buttons
                GUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Save Event"))
                {
                    manager.SaveEvent(i, ev);
                }

                if (GUILayout.Button("Remove Event"))
                {
                    removeIndex = i;
                }
                
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.Space(25);
            }

            // Removes the flagged event from both the manager and local list
            if (removeIndex >= 0)
            {
                manager.RemoveEvent(removeIndex);
                _events.RemoveAt(removeIndex);
            }

            // Add new event
            if (GUILayout.Button("Add New Event"))
            {
                var newEvt = new EasyEvent
                {
                    eventName        = "",
                    eventDescription = "",
                    Conditions       = new List<EasyCondition>(),
                    Actions          = new List<EasyAction>()
                };
                manager.AddEvent(newEvt);
                _events.Add(newEvt);
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

         

        private EasyEventManager GetEasyEventManager()
        {
            if (_easyEventManager == null)
            {
                _easyEventManager = FindObjectOfType<EasyEventManager>();
                if (_easyEventManager == null)
                    _easyEventManager = new GameObject("EasyEventManager").AddComponent<EasyEventManager>();
            }
            return _easyEventManager;
        }
    }
}
