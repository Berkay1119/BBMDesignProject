using UnityEditor;

namespace UI {
    public static class HierarchyMenuExtension {
        
        [MenuItem("GameObject/🎮 Easy Prototyping 🎮", false, 10)]
        private static void OpenEasyPrototypingPanel() {
            MainPanel.ShowWindow();
        }
    }
}