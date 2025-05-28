using UnityEditor;

namespace UI {
    public static class HierarchyMenuExtension {
        
        [MenuItem("GameObject/🎮 Easy Prototyping Object 🎮", false, 10)]
        private static void OpenEasyPrototypingPanel() {
            AddEasyObjectWindow.ShowWindow();
        }
        
        [MenuItem("GameObject/🎮 Easy Prototyping UI 🎮", false, 10)]
        private static void OpenEasyPrototypingUIPanel() {
            AddUIPanel.ShowWindow();
        }
    }
}