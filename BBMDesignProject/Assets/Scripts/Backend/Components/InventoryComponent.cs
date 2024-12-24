using System;
using System.Collections.Generic;
using Backend.UIComponents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Backend.Components
{
    public class InventoryComponent:BaseComponent
    {
        [SerializeField] private List<ItemStack> items = new List<ItemStack>();
        
        public IReadOnlyList<ItemStack> Items => items;
        public override void SetupComponent()
        {
            SetName("Inventory");
            SetDescription("This component allows the entity to store items.");
        }
        
        public void AddItem(CollectibleType type, int amount)
        {
            foreach (var item in items)
            {
                if (item.type == type)
                {
                    item.amount += amount;
                                        
                }
            }
            items.Add(new ItemStack
            {
                amount = amount,
                type = type
            });
            var ui=FindObjectsOfType<UICollectibleAmountDisplay>();

            foreach (var display in ui)
            {
                display.UpdateText();
            }
            return;
        }
    }

    [Serializable]
    public class ItemStack
    {
        public int amount;
        public CollectibleType type;
    }
}