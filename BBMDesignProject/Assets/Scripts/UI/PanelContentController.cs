using System.Collections;
using System.Collections.Generic;
using Backend.Objects;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class PanelContentController
    {
        private static PanelContentController _instance;
        public static PanelContentController Instance => _instance ??= new PanelContentController();

        private List<ObjectOnPanel> _objectsOnPanel = new List<ObjectOnPanel>();
        private ObjectOnPanel _selectedObject;


        public void AddObjectOnPanel(ObjectOnPanel objectOnPanel)
        {
            _objectsOnPanel.Add(objectOnPanel);
        }

        public void DrawPanelContent()
        {
            foreach (var objectOnPanel in _objectsOnPanel)
            {
                if (objectOnPanel.HasSprite)
                {
                    Texture2D texture = objectOnPanel.GetTexture();
                    if (texture)
                    {
                        // Apply grid offset to each object's position
                        Rect rect = new Rect(objectOnPanel.Position.x, objectOnPanel.Position.y, texture.width, texture.height);
                        GUI.DrawTexture(rect, texture);

                        HandleDragging(rect, objectOnPanel);
                    }
                }
            }
        }

        private void HandleDragging(Rect rect, ObjectOnPanel objectOnPanel)
        {
            Event currentEvent = Event.current;

            if (rect.Contains(currentEvent.mousePosition))
            {
                // Detect left-click to start dragging
                if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0) 
                {
                    GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                }
            }

            // Handle the actual dragging when left mouse button is held
            if (currentEvent.type == EventType.MouseDrag && currentEvent.button == 0 && GUIUtility.hotControl != 0)
            {
                objectOnPanel.Position = currentEvent.mousePosition - new Vector2(rect.width / 2, rect.height / 2);
                currentEvent.Use();
            }

            // Release dragging when left mouse button is released
            if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0 && GUIUtility.hotControl != 0)
            {
                GUIUtility.hotControl = 0;
            }
            
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && rect.Contains(currentEvent.mousePosition))
            {
                // Show context menu
                ShowContextMenu(objectOnPanel);
                currentEvent.Use(); // Mark the event as used
            }
        }

        private void ShowContextMenu(ObjectOnPanel objectOnPanel)
        {
            GenericMenu menu = new GenericMenu();

            // Display object information
            menu.AddDisabledItem(new GUIContent($"Name: {objectOnPanel.Name}"));
            menu.AddDisabledItem(new GUIContent($"Position: {objectOnPanel.Position}"));

            // Option to delete the object
            menu.AddItem(new GUIContent("Delete Object"), false, () => RemoveObjectOnPanel(objectOnPanel));

            menu.ShowAsContext();
        }

        private void RemoveObjectOnPanel(ObjectOnPanel objectOnPanel)
        {
            _objectsOnPanel.Remove(objectOnPanel);
        }
    }
}