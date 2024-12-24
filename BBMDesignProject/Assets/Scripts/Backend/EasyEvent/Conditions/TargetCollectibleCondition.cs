using Backend.Attributes;
using Backend.Components;
using UnityEngine;

namespace Backend.Conditions
{
    [Condition]
    public class TargetCollectibleCondition:EasyCondition
    {
        public CollectibleType CollectibleType;
        public GameObject Target;
        public int Amount;
        public override void Setup(EasyAction action)
        {
            if (Target.GetComponent<InventoryComponent>()==null)
            {
                Target.AddComponent<InventoryComponent>();
            }
            Target.GetComponent<InventoryComponent>().BindAfterChange(action, CollectibleType, Amount);
        }
        
        public TargetCollectibleCondition()
        {
            conditionName = "TargetCollectibleCondition";
            conditionDescription = "Check if a target has a certain amount of a collectible";
        }
        
        public override void DrawGUI()
        {
            base.DrawGUI();
            Target = (GameObject)UnityEditor.EditorGUILayout.ObjectField("Target", Target, typeof(GameObject), true);
            CollectibleType = (CollectibleType)UnityEditor.EditorGUILayout.EnumPopup("Collectible Type", CollectibleType);
            Amount = UnityEditor.EditorGUILayout.IntField("Amount", Amount);
            GUILayout.EndVertical();
        }
    }
}