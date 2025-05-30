using Backend.CustomVariableFeature;
using UnityEngine;
using UnityEngine.UI;

namespace Backend.Components
{
    public class GameWorldBarComponent:BaseComponent
    {
        [SerializeField] private SerializableCustomVariable currentValueToDisplay;
        [SerializeField] private SerializableCustomVariable maxValueToDisplay;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Color fillColor;

        private Image _fillImage;
        private GameObject _healthBarUI;
        
        public override void SetupComponent()
        {
            PrepareHealthBar();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            currentValueToDisplay.OnValueChanged += UpdateBarDisplay;
            maxValueToDisplay.OnValueChanged += UpdateBarDisplay;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            currentValueToDisplay.OnValueChanged -= UpdateBarDisplay;
            maxValueToDisplay.OnValueChanged -= UpdateBarDisplay;
        }

        private void PrepareHealthBar()
        {
            
        }

        private void UpdateBarDisplay()
        {
            
        }
    }
}