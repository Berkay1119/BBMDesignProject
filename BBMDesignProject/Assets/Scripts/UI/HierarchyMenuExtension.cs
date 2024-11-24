using UnityEditor;

namespace UI {
    public static class HierarchyMenuExtension {
        
        [MenuItem("GameObject/🎮 Easy Prototyping Object 🎮", false, 10)]
        private static void OpenEasyPrototypingPanel() {
            AddObjectWindow.ShowWindow();
        }
    }
}