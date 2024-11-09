using UnityEditor;
using UnityEngine;

public class MainPanel : EditorWindow
{
    
    [MenuItem("Custom UI/Easy Prototyping")]
    public static void ShowWindow()
    {
        // Open the window
        GetWindow<MainPanel>("Easy Prototyping Panel");
    }

    private void OnGUI()
    {
        // The content of the easy prototyping main window
        GUILayout.BeginVertical("box");
        GUI.backgroundColor = Color.grey;

        GUILayout.Label("This is a custom editor panel", EditorStyles.boldLabel);

        GUILayout.EndVertical();
    }
}