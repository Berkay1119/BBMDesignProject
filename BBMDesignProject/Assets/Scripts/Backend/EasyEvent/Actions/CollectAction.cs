using Backend.Attributes;
using Backend.Components;
using UnityEditor;
using UnityEngine;

namespace Backend.Actions
{
    [Action]
    public class CollectAction:EasyAction
    {
        public GameObject Target;
        public string ItemName;
        public int Amount;
        
        public CollectAction()
        {
            actionName = "Collect";
            actionDescription = "Collects an item";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            Target = (GameObject)EditorGUILayout.ObjectField("Target", Target, typeof(GameObject), true);
            Amount = EditorGUILayout.IntField("Amount", Amount);
            GUILayout.EndVertical();
        }

        public override void Execute()
        {
            if (Target.GetComponent<InventoryComponent>()==null)
            {
                Target.AddComponent<InventoryComponent>();
            }
            Target.GetComponent<InventoryComponent>().AddItem(ItemName, Amount);
        }
    }
}