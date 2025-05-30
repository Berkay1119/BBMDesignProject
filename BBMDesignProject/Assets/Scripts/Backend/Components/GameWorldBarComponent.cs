using Backend.Attributes;
using Backend.CustomVariableFeature;
using UnityEngine;
using UnityEngine.UI;

namespace Backend.Components
{
    
    [Component]
    [AddComponentMenu("EasyPrototyping/Game World Bar Component")]
    public class GameWorldBarComponent : BaseComponent
    {
        [SerializeField] private SerializableCustomVariable currentValue;
        [SerializeField] private SerializableCustomVariable maxValue;
        [SerializeField] private Color backgroundColor = Color.gray;
        [SerializeField] private Color fillColor = Color.red;

        [SerializeField] private Image _fillImage;
        [SerializeField] private GameObject _valueBarUI;

        public override void SetupComponent()
        {
            PrepareValueBar();
        }
        
        public GameWorldBarComponent()
        {
            SetName("Game World Bar");
            SetDescription("This component displays a value bar in the game world, showing current and maximum values.");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            currentValue.OnValueChanged += UpdateBarDisplay;
            maxValue.OnValueChanged += UpdateBarDisplay;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            currentValue.OnValueChanged -= UpdateBarDisplay;
            maxValue.OnValueChanged -= UpdateBarDisplay;
        }

        private void PrepareValueBar()
        {
            // Create a world-space canvas
            GameObject canvasGO = new GameObject("ValueBarCanvas");
            canvasGO.transform.SetParent(transform);
            canvasGO.transform.localPosition = Vector3.zero ;
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.scaleFactor = 0.01f;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Background
            GameObject backgroundGO = new GameObject("Background");
            backgroundGO.transform.SetParent(canvasGO.transform, false);
            Image bgImage = backgroundGO.AddComponent<Image>();
            bgImage.color = backgroundColor;
            RectTransform bgRect = backgroundGO.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(100, 10);

            // Fill
            GameObject fillGO = new GameObject("Fill");
            fillGO.transform.SetParent(backgroundGO.transform, false);
            _fillImage = fillGO.AddComponent<Image>();
            _fillImage.color = fillColor;
            _fillImage.type = Image.Type.Filled;
            _fillImage.fillMethod = Image.FillMethod.Horizontal;

            RectTransform fillRect = fillGO.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0, 0);
            fillRect.anchorMax = new Vector2(1, 1);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            _valueBarUI = canvasGO;
            _valueBarUI.transform.localScale = Vector3.one * 0.1f;

            UpdateBarDisplay();
        }

        private void UpdateBarDisplay()
        {
            float current = float.Parse(currentValue._value);
            float max = float.Parse(maxValue._value);

            float fillAmount = (max > 0f) ? Mathf.Clamp01(current / max) : 0f;

            if (_fillImage != null)
                _fillImage.fillAmount = fillAmount;
        }
    }
}
