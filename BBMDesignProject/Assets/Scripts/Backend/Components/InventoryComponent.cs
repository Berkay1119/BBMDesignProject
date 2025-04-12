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
        private List<(CollectibleType,int,EasyAction)> _afterChangeTargets = new List<(CollectibleType, int, EasyAction)>();
        
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

            foreach (var item in items)
            {
                if (CheckForItem(item.type, item.amount))
                {
                    foreach (var (type1, amount1, action) in _afterChangeTargets)
                    {
                        if (type1 == item.type && amount1 <= item.amount)
                        {
                            action.Execute();
                        }
                    }
                }
            }
        }

        public void BindAfterChange(EasyAction action, CollectibleType type, int amount)
        {
            _afterChangeTargets.Add((type, amount, action));
        }
        
        private bool CheckForItem(CollectibleType type, int amount)
        {
            foreach (var item in items)
            {
                if (item.type == type)
                {
                    return item.amount >= amount;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class ItemStack
    {
        public int amount;
        public CollectibleType type;
    }
}