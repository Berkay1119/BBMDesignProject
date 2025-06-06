using System.Linq;
using Backend.CustomVariableFeature;
using Backend.UIComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AddUIPanel : EditorWindow
    {
        private static UIPanelSettings _settings;
        private string[] _availableVariables = new string[0];
        private int _selectedVariableIndex = 0;

        public static void ShowWindow()
        {
            GetWindow<AddUIPanel>("Add New UI Panel");
            _settings = new UIPanelSettings();
        }

        private void OnGUI()
        {
            GUILayout.Label("UI Panel Settings", EditorStyles.boldLabel);

            GUILayout.BeginVertical("box");
            _settings.PanelName = EditorGUILayout.TextField("Panel Name: ", _settings.PanelName);
            _settings.Position = (UIPosition)EditorGUILayout.EnumPopup("Position: ", _settings.Position);
            _settings.Size = EditorGUILayout.IntField("Size (1-10): ", _settings.Size);
            _settings.BackgroundImage = (Sprite)EditorGUILayout.ObjectField("Background Image: ", _settings.BackgroundImage, typeof(Sprite), true);
            _settings.IconImage = (Sprite)EditorGUILayout.ObjectField("Icon:", _settings.IconImage, typeof(Sprite), true);
            _settings.TrackingObject = (GameObject)EditorGUILayout.ObjectField("Tracking Object: ", _settings.TrackingObject, typeof(GameObject), true);

            if (_settings.TrackingObject != null)
            {
                _availableVariables = _settings.TrackingObject
                    .GetComponents<SerializableCustomVariable>()
                    .Select(v => v.Name)
                    .ToArray();

                if (_availableVariables.Length > 0)
                {
                    _selectedVariableIndex = Mathf.Clamp(_selectedVariableIndex, 0, _availableVariables.Length - 1);
                    _selectedVariableIndex = EditorGUILayout.Popup("Tracked Variable:", _selectedVariableIndex, _availableVariables);
                    _settings.TrackedVariableName = _availableVariables[_selectedVariableIndex];
                    _settings.DisplayText = EditorGUILayout.TextField("Display Text: ", _settings.DisplayText);
                }
                else
                {
                    EditorGUILayout.HelpBox("No custom variables found on selected object.", MessageType.Warning);
                }
            }

            GUILayout.EndVertical();

            if (GUILayout.Button("Add"))
            {
                CreateNewUIPanel();
            }
        }

        private void CreateNewUIPanel()
        {
            if (string.IsNullOrEmpty(_settings.PanelName) || _settings.TrackingObject == null || string.IsNullOrEmpty(_settings.TrackedVariableName))
            {
                Debug.LogWarning("Please ensure Panel Name, Tracking Object and Tracked Variable are assigned.");
                return;
            }
            
            if (GameObject.Find("EasyCanvas") == null)
            {
                var canvas = new GameObject("EasyCanvas");
                canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                
                var scaler = canvas.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvas.AddComponent<GraphicRaycaster>();
                
                // Put the Canvas on top in the hierarchy
                canvas.transform.SetSiblingIndex(0);
            }

            var panel = new GameObject(_settings.PanelName);
            panel.transform.SetParent(GameObject.Find("EasyCanvas").transform);
            panel.AddComponent<RectTransform>();

            if (_settings.BackgroundImage != null)
            {
                var img = panel.AddComponent<Image>();
                img.sprite = _settings.BackgroundImage;
            }

            var rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.anchorMin = GetAnchorMin(_settings.Position);
            rectTransform.anchorMax = GetAnchorMax(_settings.Position);
            rectTransform.pivot = GetPivot(_settings.Position);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one * _settings.Size;

            var display = panel.AddComponent<UICustomVariableDisplay>();
            display.target = _settings.TrackingObject;
            display.variableName = _settings.TrackedVariableName;
            display.userInputText = _settings.DisplayText;

            if (_settings.IconImage != null)
            {
                GameObject iconObject = new GameObject("Icon");
                iconObject.transform.SetParent(panel.transform);

                var iconRect = iconObject.AddComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0f, 0f);   // bottom-left
                iconRect.anchorMax = new Vector2(1f, 1f);   // top-right
                iconRect.offsetMin = Vector2.zero;         // remove padding
                iconRect.offsetMax = Vector2.zero;
                iconRect.pivot = new Vector2(0.5f, 0.5f);
                iconRect.localScale = Vector3.one;

                var iconImage = iconObject.AddComponent<Image>();
                iconImage.sprite = _settings.IconImage;
                iconImage.preserveAspect = true;
            }
            
            
            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(panel.transform);
            var textRect = textObject.AddComponent<RectTransform>();
            
            // Set to stretch/stretch
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.localScale = Vector3.one;
            
            var text = textObject.AddComponent<TMPro.TextMeshProUGUI>();
            text.alignment = TMPro.TextAlignmentOptions.Center;
            display.text = text;
            
            // Set initial text value
            var trackedVar = _settings.TrackingObject
                .GetComponents<SerializableCustomVariable>()
                .FirstOrDefault(v => v.Name == _settings.TrackedVariableName);

            // Set initial text value
            if (trackedVar != null)
            {
                if (!string.IsNullOrWhiteSpace(_settings.DisplayText))
                {
                    text.text = $"{_settings.DisplayText}: {trackedVar._value}";
                }
                else
                {
                    text.text = $"{trackedVar._value}";
                }
            }
            
            Close();
        }

        private Vector2 GetPivot(UIPosition pos) => pos switch
        {
            UIPosition.RightTop => new Vector2(1, 1),
            UIPosition.RightBottom => new Vector2(1, 0),
            UIPosition.LeftTop => new Vector2(0, 1),
            UIPosition.LeftBottom => new Vector2(0, 0),
            UIPosition.Center => new Vector2(0.5f, 0.5f),
            _ => Vector2.zero
        };

        private Vector2 GetAnchorMin(UIPosition pos) => GetPivot(pos);
        private Vector2 GetAnchorMax(UIPosition pos) => GetPivot(pos);
    }

    public struct UIPanelSettings
    {
        public string PanelName;
        public UIPosition Position;
        public int Size;
        public Sprite BackgroundImage;
        public Sprite IconImage;
        public GameObject TrackingObject;
        public string TrackedVariableName;
        public string DisplayText;
    }

    public enum UIPosition
    {
        RightTop,
        RightBottom,
        LeftTop,
        LeftBottom,
        Center
    }
}
