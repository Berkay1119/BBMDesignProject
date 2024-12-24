using System;
using Backend.Components;
using Backend.UIComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AddUIPanel:EditorWindow
    {
        private static UIPanelSettings _settings;
        public static void ShowWindow() {
            GetWindow<AddUIPanel>("Add New UI Panel");
            _settings = new UIPanelSettings();
        }

        private void OnGUI()
        {
            GUILayout.Label("UI Panel Settings", EditorStyles.boldLabel);
            
            GUILayout.BeginVertical("box");
            _settings.PanelName = EditorGUILayout.TextField("Panel Name: ", _settings.PanelName);
            _settings.Position = (UIPosition)EditorGUILayout.EnumPopup("Position: ", _settings.Position);
            _settings.Size = EditorGUILayout.IntField("Size: ", _settings.Size);
            _settings.BackgroundImage = (Sprite)EditorGUILayout.ObjectField("Background Image: ", _settings.BackgroundImage, typeof(Sprite), true);
            _settings.Type = (UIType)EditorGUILayout.EnumPopup("Type: ", _settings.Type);
            
            if (_settings.Type == UIType.CollectibleAmountDisplay)
            {
                _settings.TrackedCollectible = (CollectibleType)EditorGUILayout.EnumPopup("Tracked Collectible: ", _settings.TrackedCollectible);
                _settings.TrackingObject = (GameObject)EditorGUILayout.ObjectField("Tracking Object: ", _settings.TrackingObject, typeof(GameObject), true);
            }
            else if (_settings.Type == UIType.TextDisplay)
            {
                _settings.Text = EditorGUILayout.TextField("Text: ", _settings.Text);
            }
            GUILayout.EndVertical();
            
            if (GUILayout.Button("Add"))
            {
                CreateNewUIPanel();
            }
        }

        private void CreateNewUIPanel()
        {
            //Create canvas if it doesn't exist
            if (GameObject.Find("EasyCanvas") == null)
            {
                var canvas = new GameObject("EasyCanvas");
                canvas.AddComponent<Canvas>();
                canvas.AddComponent<CanvasScaler>();
                canvas.AddComponent<GraphicRaycaster>();
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }
            
            //Create panel
            var panel = new GameObject(_settings.PanelName);
            panel.transform.SetParent(GameObject.Find("EasyCanvas").transform);
            panel.AddComponent<RectTransform>();
            if (_settings.BackgroundImage != null)
            {
                panel.AddComponent<Image>();
                panel.GetComponent<Image>().sprite = _settings.BackgroundImage;
            }

            
            //Set position
            panel.GetComponent<RectTransform>().anchorMin = GetAnchorMin(_settings.Position);
            panel.GetComponent<RectTransform>().anchorMax = GetAnchorMax(_settings.Position);
            panel.GetComponent<RectTransform>().pivot = GetPivot(_settings.Position);
            panel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            panel.GetComponent<RectTransform>().localScale = Vector3.one*(_settings.Size);
            
            
            //Add UI component
            switch (_settings.Type)
            {
                case UIType.CollectibleAmountDisplay:
                    var collectibleAmountDisplay = panel.AddComponent<UICollectibleAmountDisplay>();
                    collectibleAmountDisplay.type = _settings.TrackedCollectible;
                    collectibleAmountDisplay.target = _settings.TrackingObject;
                    GameObject textObject = new GameObject("Text");
                    var rect = textObject.AddComponent<RectTransform>();
                    textObject.transform.SetParent(panel.transform);
                    rect.anchoredPosition = Vector2.zero;
                    rect.pivot = GetPivot(_settings.Position);
                    var text = textObject.AddComponent<TMPro.TextMeshProUGUI>();
                    textObject.GetComponent<RectTransform>().localScale = Vector3.one;
                    collectibleAmountDisplay.text = text;
                    break;
                case UIType.TextDisplay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector2 GetPivot(UIPosition settingsPosition)
        {
            switch (settingsPosition)
            {
                case UIPosition.RightTop:
                    return new Vector2(1, 1);
                case UIPosition.RightBottom:
                    return new Vector2(1, 0);
                case UIPosition.LeftTop:
                    return new Vector2(0, 1);
                case UIPosition.LeftBottom:
                    return new Vector2(0, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector2 GetAnchorMax(UIPosition settingsPosition)
        {
            switch (settingsPosition)
            {
                case UIPosition.RightTop:
                    return new Vector2(1, 1);
                case UIPosition.RightBottom:
                    return new Vector2(1, 0);
                case UIPosition.LeftTop:
                    return new Vector2(0, 1);
                case UIPosition.LeftBottom:
                    return new Vector2(0, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Vector2 GetAnchorMin(UIPosition settingsPosition)
        {
            switch (settingsPosition)
            {
                case UIPosition.RightTop:
                    return new Vector2(1, 1);
                case UIPosition.RightBottom:
                    return new Vector2(1, 0);
                case UIPosition.LeftTop:
                    return new Vector2(0, 1);
                case UIPosition.LeftBottom:
                    return new Vector2(0, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public struct UIPanelSettings
    {
        public string PanelName;
        public UIPosition Position;
        public int Size;
        public Sprite BackgroundImage;
        public UIType Type;
        public CollectibleType TrackedCollectible;
        public GameObject TrackingObject;
        public string Text;
    }

    public enum UIType
    {
        CollectibleAmountDisplay,
        TextDisplay,
    }

    public enum UIPosition
    {
        RightTop,
        RightBottom,
        LeftTop,
        LeftBottom
    }
}