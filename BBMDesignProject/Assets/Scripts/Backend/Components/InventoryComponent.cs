using System;
using System.Collections.Generic;
using Backend.EasyEvent;
using Backend.UIComponents;
using UnityEngine;

namespace Backend.Components
{
    public class InventoryComponent:BaseComponent
    {
        [SerializeField] private List<ItemStack> items = new List<ItemStack>();
        private List<(CollectibleType type, int threshold, EasyAction action)> _afterChangeTargets = new List<(CollectibleType, int, EasyAction)>();
        
        public IReadOnlyList<ItemStack> Items => items;
        public override void SetupComponent()
        {
            SetName("Inventory");
            SetDescription("This component allows the entity to store items.");
        }
        
        public void AddItem(CollectibleType type, int amount)
        {
            // Var olan stack'leri güncelle
            bool found = false;
            foreach (var item in items)
            {
                if (item.type == type)
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                items.Add(new ItemStack { type = type, amount = amount });
            }

            // UI güncelle
            foreach (var display in FindObjectsOfType<UICollectibleAmountDisplay>())
                display.UpdateText();

            // After-change targetları tetikle
            foreach (var item in items)
            {
                if (CheckForItem(item.type, item.amount))
                {
                    foreach (var (tType, threshold, action) in _afterChangeTargets)
                    {
                        if (tType == item.type && item.amount >= threshold)
                        {
                            // --> Yeni Invoke imzası
                            action.Execute(this, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bir custom action'ı, belirli bir CollectibleType ve miktar eşiği ile bağlar.
        /// </summary>
        public void BindAfterChange(EasyAction action, CollectibleType type, int amount)
        {
            _afterChangeTargets.Add((type, amount, action));
        }

        private bool CheckForItem(CollectibleType type, int amount)
        {
            foreach (var item in items)
                if (item.type == type)
                    return item.amount >= amount;
            return false;
        }
    }

    [Serializable]
    public class ItemStack
    {
        public CollectibleType type;
        public int amount;
    }
}