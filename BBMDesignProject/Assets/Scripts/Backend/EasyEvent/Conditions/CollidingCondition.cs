using System;
using Backend.Attributes;
using Backend.Components;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Backend.Conditions
{
    [Condition]
    public class CollidingCondition:EasyCondition
    {
        public CollectibleType CollectibleType;
        public string collecterTag;
        
        public CollidingCondition()
        {
            conditionName = "CollidingCondition";
            conditionDescription = "Check if two objects are colliding";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Collectible Type:", GUILayout.Width(150));
            CollectibleType= (CollectibleType)EditorGUILayout.EnumPopup(CollectibleType);
            GUILayout.EndHorizontal();
            collecterTag = EditorGUILayout.TextField("Collecter Tag", collecterTag);
            GUILayout.EndVertical();
        }

        public override void Setup(EasyAction action)
        {
            var collectibles = UnityEngine.Object.FindObjectsOfType<CollectibleComponent>();
            var collecters = GameObject.FindGameObjectsWithTag(collecterTag);
            foreach (var obj1 in collectibles)
            {
                foreach (var obj2 in collecters)
                {
                    if (obj1.GetComponent<CollisionEventComponent>()==null)
                    {
                        obj1.AddComponent<CollisionEventComponent>();
                    }
                    obj1.GetComponent<CollisionEventComponent>().AddTarget(obj2,action);
                }
            }

        }
    }
}