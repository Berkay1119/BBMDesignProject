using UnityEditor;
using UnityEngine;

namespace UI
{
    public class MainPanel : EditorWindow
    {
        private static Texture2D lightGreyTexture;
        private static PanelContentController _panelContentController;

        private Vector2 gridOffset; 
        private Vector2 dragStartPos;
        private bool isDragging = false;
        private const float gridSpacing = 20f;
        private Vector2 rightPanelScrollPosition;

        private void OnEnable()
        {
            _panelContentController = PanelContentController.Instance;
        }

        [MenuItem("Custom UI/Easy Prototyping")]
        public static void ShowWindow()
        {
            // open the window
            GetWindow<MainPanel>("Easy Prototyping Panel");

            // create light grey texture if not already created
            if (lightGreyTexture == null)
            {
                lightGreyTexture = new Texture2D(1, 1);
                lightGreyTexture.SetPixel(0, 0, new Color(211f / 255f, 211f / 255f, 211f / 255f));
                lightGreyTexture.Apply();
            }
        }

        private void OnGUI()
        {
            float leftPanelWidth = position.width * 0.8f;
            float rightPanelWidth = position.width * 0.2f;
            
            GUILayout.BeginArea(new Rect(0, 0, leftPanelWidth - 2, position.height), EditorStyles.helpBox);
            //HandleGridDragging();
            DrawGrid(leftPanelWidth, position.height);
            _panelContentController.DrawPanelContent();
            GUILayout.EndArea();

            DrawRightPanel(leftPanelWidth, rightPanelWidth);
        }

        private void DrawGrid(float width, float height)
        {
            Handles.color = Color.gray;

            for (float x = gridOffset.x % gridSpacing; x < width; x += gridSpacing)
                Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, height, 0));
            for (float y = gridOffset.y % gridSpacing; y < height; y += gridSpacing)
                Handles.DrawLine(new Vector3(0, y, 0), new Vector3(width, y, 0));
        }

        private void HandleGridDragging()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                isDragging = true;
                dragStartPos = currentEvent.mousePosition;
            }
            else if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
            {
                isDragging = false;
            }

            if (isDragging && currentEvent.type == EventType.MouseDrag)
            {
                Vector2 dragDelta = currentEvent.mousePosition - dragStartPos;
                gridOffset += dragDelta;
                dragStartPos = currentEvent.mousePosition;
                Repaint();
            }
        }

        private void DrawRightPanel(float leftPanelWidth, float rightPanelWidth)
        {
            EditorGUI.DrawRect(new Rect(leftPanelWidth - 2, 0, 4, position.height), Color.grey);

            GUIStyle rightPanelStyle = new GUIStyle { normal = { background = lightGreyTexture } };
            GUILayout.BeginArea(new Rect(leftPanelWidth + 2, 0, rightPanelWidth - 2, position.height), rightPanelStyle);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.black } };
            if (GUILayout.Button("Add Object", buttonStyle, GUILayout.Height(30)))
            {
                Debug.Log("Add Object button clicked");
                AddObjectWindow.ShowWindow();
            }

            if (GUILayout.Button("Play", buttonStyle, GUILayout.Height(30)))
            {
                Debug.Log("Play is button clicked");
            }
            
            if (GUILayout.Button("Save as JSON", buttonStyle, GUILayout.Height(30)))
            {
                Debug.Log("Prototype has been saved as JSON");
            }

            GUILayout.EndArea();
        }
    }
}