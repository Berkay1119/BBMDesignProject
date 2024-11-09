using UnityEditor;
using UnityEngine;

namespace UI {
    public class MainPanel : EditorWindow
    {
        private static Texture2D lightGreyTexture;

        [MenuItem("Custom UI/Easy Prototyping")]
        public static void ShowWindow()
        {
            // Open the window
            GetWindow<MainPanel>("Easy Prototyping Panel");

            // Create light grey texture if not already created
            if (lightGreyTexture == null)
            {
                lightGreyTexture = new Texture2D(1, 1);
                lightGreyTexture.SetPixel(0, 0, new Color(211f / 255f, 211f / 255f, 211f / 255f));
                lightGreyTexture.Apply();
            }
        }

        private void OnGUI()
        {
            // Set the inner window scales
            float leftPanelWidth = position.width * 0.8f;
            float rightPanelWidth = position.width * 0.2f;

            // Left panel
            GUILayout.BeginArea(new Rect(0, 0, leftPanelWidth - 2, position.height), EditorStyles.helpBox);
            GUILayout.EndArea();

            // Borderline between left and right panels
            EditorGUI.DrawRect(new Rect(leftPanelWidth - 2, 0, 4, position.height), Color.grey);

            // Right panel background color
            GUIStyle rightPanelStyle = new GUIStyle();
            rightPanelStyle.normal.background = lightGreyTexture;

            // Right panel
            GUILayout.BeginArea(new Rect(leftPanelWidth + 2, 0, rightPanelWidth - 2, position.height), rightPanelStyle);

            // Button styles
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.black;

            if (GUILayout.Button("Add Object", buttonStyle, GUILayout.Height(30)))
            {
                Debug.Log("Add Object button clicked");
            }

            if (GUILayout.Button("Delete Object", buttonStyle, GUILayout.Height(30)))
            {
                Debug.Log("Delete Object button clicked");
            }

            GUILayout.EndArea();
        }
    }
}
