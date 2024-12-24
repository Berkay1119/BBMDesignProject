using Backend.Components;
using UnityEngine;

namespace Backend.UIComponents
{
    public class UICollectibleAmountDisplay:MonoBehaviour
    {
        public CollectibleType type;
        public GameObject target;
        public TMPro.TextMeshProUGUI text;
        
        private void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            if (target.GetComponent<InventoryComponent>() == null)
            {
                text.text = type.ToString() + ": 0";
                return;
            }

            foreach (var item in target.GetComponent<InventoryComponent>().Items)
            {
                if (item.type != type) continue;
                text.text = type.ToString() + ": " + item.amount;
                return;
            }
        }
    }
}