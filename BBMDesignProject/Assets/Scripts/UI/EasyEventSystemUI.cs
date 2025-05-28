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

        [MenuItem("Window/🎮 Easy Event System 🎮")]
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

            // Is Unity in playmode?
            bool isPlaying = EditorApplication.isPlaying;
            
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

                GUILayout.Space(20);
                // Add Condition
                if (EditorGUILayout.DropdownButton(new GUIContent("Add New Condition"), FocusType.Keyboard))
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
                
                int removeConditionIndex = -1;
                
                for (int c = 0; c < ev.Conditions.Count; c++)
                {
                    var cond = ev.Conditions[c];
                    
                    // Vertical wrapper for indent 
                    GUILayout.BeginVertical();
                    GUILayout.Space(5);    // Spacing between condition boxes
                    
                    GUILayout.BeginHorizontal();  // Wrapper for indent
                    GUILayout.Space(20);    // Left indent (20px)
                    
                    Color prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = new Color(0.8f, 0.9f, 1f);
                    GUILayout.BeginVertical("box");   // Frame/box
                    
                    // Condition title
                    GUILayout.Label(cond.conditionName ?? cond.GetType().Name, EditorStyles.boldLabel);

                    cond.DrawGUI();
                    
                    GUILayout.Space(5);
                    
                    GUI.enabled = !isPlaying;
                    
                    // REMOVE button 
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        removeConditionIndex = c;
                    }
                    
                    GUI.enabled = true;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    GUILayout.EndVertical();     // Box
                    GUI.backgroundColor = prevColor;
                    GUILayout.EndHorizontal();  // Indent wrapper for row
                    GUILayout.EndVertical();   // Wrapper spacing
                }
                
                if (removeConditionIndex >= 0)
                {
                    ev.Conditions.RemoveAt(removeConditionIndex);
                }

                GUILayout.Space(20);
                // Add Action
                if (EditorGUILayout.DropdownButton(new GUIContent("Add New Action"), FocusType.Keyboard))
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
                
                int removeActionIndex = -1;
                
                for (int a = 0; a < ev.Actions.Count; a++)
                {
                    var act = ev.Actions[a];
                    
                    GUILayout.BeginVertical();    // Vertical wrapper for indent  
                    GUILayout.Space(5);      // Spacing between action boxes
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);     // Left indent
                    
                    Color prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = new Color(0.8f, 0.9f, 1f); 
                    
                    GUILayout.BeginVertical("box");
                    
                    // Action title
                    GUILayout.Label(act.actionName ?? act.GetType().Name, EditorStyles.boldLabel);
  
                    act.DrawGUI();
                    
                    GUILayout.Space(5);
                    
                    // Disable buttons during play mode
                    
                    GUI.enabled = !isPlaying;
                    
                    // REMOVE button 
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        removeActionIndex = a;
                    }
                    
                    GUI.enabled = true;     // Enable the rest of the UI
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();        // Box
                    GUI.backgroundColor = prevColor;
                    
                    GUILayout.EndHorizontal();     // Indent row
                    GUILayout.EndVertical();       // Wrapper spacing
                }
                
                if (removeActionIndex >= 0)
                {
                    ev.Actions.RemoveAt(removeActionIndex);
                }

                GUILayout.Space(20); 
                
                EditorGUILayout.HelpBox("Don't forget to save the event after making any changes or updates!", MessageType.Info);
                
                // Save and Remove buttons
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                float buttonWidth = position.width * 0.25f;
                
                // Disable buttons during play mode
                GUI.enabled = !isPlaying;
                
                if (GUILayout.Button("Save Event", GUILayout.Width(buttonWidth)))
                {
                    manager.SaveEvent(i, ev);
                }

                if (GUILayout.Button("Remove Event", GUILayout.Width(buttonWidth)))
                {
                    removeIndex = i;
                }
                
                GUI.enabled = true;   // Enable the rest of the UI
                
                GUILayout.FlexibleSpace();
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
