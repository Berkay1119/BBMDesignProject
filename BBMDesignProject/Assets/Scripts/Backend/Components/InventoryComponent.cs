using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Backend.Components
{
    public class InventoryComponent:BaseComponent
    {
        [SerializeField] private List<ItemStack> items = new List<ItemStack>();
        
        public override void SetupComponent()
        {
            SetName("Inventory");
            SetDescription("This component allows the entity to store items.");
        }
        
        public void AddItem(string nameKey, int amount)
        {
            foreach (var item in items)
            {
                if (item.name == nameKey)
                {
                    item.amount += amount;
                    return;
                }
            }
            items.Add(new ItemStack
            {
                amount = amount,
                name = nameKey
            });
        }
    }

    [Serializable]
    public class ItemStack
    {
        public int amount;
        public string name;
    }
}