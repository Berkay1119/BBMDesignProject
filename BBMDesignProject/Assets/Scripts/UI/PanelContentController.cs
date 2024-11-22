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

        private ObjectOnPanel currentlyDraggedObject = null;

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
                        Rect rect = new Rect(objectOnPanel.Position.x, objectOnPanel.Position.y, texture.width*objectOnPanel.Scale, texture.height*objectOnPanel.Scale);
                        GUI.DrawTexture(rect, texture);

                        HandleDragging(rect, objectOnPanel);
                    }
                }
            }
        }

        private void HandleDragging(Rect rect, ObjectOnPanel objectOnPanel)
        {
            Event currentEvent = Event.current;

            // Detect left-click on object to start dragging
            if (rect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                currentlyDraggedObject = objectOnPanel; // Set the object to be dragged
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                currentEvent.Use();
            }

            // Handle the actual dragging when left mouse button is held
            if (currentlyDraggedObject != null && currentEvent.type == EventType.MouseDrag && currentEvent.button == 0 && GUIUtility.hotControl != 0)
            {
                currentlyDraggedObject.Position = currentEvent.mousePosition - new Vector2(rect.width / 2, rect.height / 2);
                currentEvent.Use();
            }

            // Release dragging when left mouse button is released
            if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0 && GUIUtility.hotControl != 0)
            {
                currentlyDraggedObject = null;
                GUIUtility.hotControl = 0;
            }

            // Show context menu on right-click within the object's rect
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 && rect.Contains(currentEvent.mousePosition))
            {
                ShowContextMenu(objectOnPanel);
                currentEvent.Use();
            }
        }

        private void ShowContextMenu(ObjectOnPanel objectOnPanel)
        {
            GenericMenu menu = new GenericMenu();

            // Display object information
            menu.AddDisabledItem(new GUIContent($"Name: {objectOnPanel.Name}"));
            menu.AddDisabledItem(new GUIContent($"Position: {objectOnPanel.Position}"));
            menu.AddDisabledItem(new GUIContent("Components"));
            menu.AddSeparator("");
            for (int i = 0; i < objectOnPanel.Components.Count; i++)
            {
                menu.AddDisabledItem(new GUIContent(objectOnPanel.Components[i].Name));
            }
            // Option to delete the object
            menu.AddItem(new GUIContent("Delete Object"), false, () => RemoveObjectOnPanel(objectOnPanel));
            
            //Scale Control
            menu.AddItem(new GUIContent("Increase Scale") , false, () => objectOnPanel.Scale+=0.1f);
            menu.AddItem(new GUIContent("Decrease Scale") , false, () => objectOnPanel.Scale-=0.1f);

            menu.ShowAsContext();
        }

        private void RemoveObjectOnPanel(ObjectOnPanel objectOnPanel)
        {
            _objectsOnPanel.Remove(objectOnPanel);
        }
    }
}