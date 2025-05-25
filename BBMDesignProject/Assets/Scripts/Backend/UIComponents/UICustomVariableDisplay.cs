using System.Linq;
using Backend.CustomVariableFeature;
using UnityEngine;

namespace Backend.UIComponents
{
    public class UICustomVariableDisplay:MonoBehaviour
    {
        public string variableName;
        public GameObject target;
        public TMPro.TextMeshProUGUI text;
        
        private SerializableCustomVariable trackedVariable;
        
        private void Start()
        {
            FindTrackedVariable();
            UpdateText();
        }

        private void Update()
        {
            if (trackedVariable == null)
            {
                FindTrackedVariable(); 
                return;
            }

            UpdateText();
        }

        private void FindTrackedVariable()
        {
            if (target == null || string.IsNullOrEmpty(variableName)) return;

            trackedVariable = target
                .GetComponents<SerializableCustomVariable>()
                .FirstOrDefault(v => v.Name == variableName);
        }

        private void UpdateText()
        {
            if (trackedVariable != null && text != null)
            {
                text.text = $"{trackedVariable.Name}: {trackedVariable._value}";
            }
        }
    }
}