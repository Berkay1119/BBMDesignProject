using System;
using Backend.Attributes;
using Backend.Components;
using UnityEngine;

namespace Backend.Conditions
{
    [Condition]
    public class CollidingCondition:EasyCondition
    {
        public string Tag1;
        public string Tag2;
        
        public CollidingCondition()
        {
            conditionName = "CollidingCondition";
            conditionDescription = "Check if two objects are colliding";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            Tag1 = UnityEditor.EditorGUILayout.TextField("Tag1", Tag1);
            Tag2 = UnityEditor.EditorGUILayout.TextField("Tag2", Tag2);
            GUILayout.EndVertical();
        }

        public override void Setup(EasyAction action)
        {
            var objects1 = GameObject.FindGameObjectsWithTag(Tag1);
            var objects2 = GameObject.FindGameObjectsWithTag(Tag2);
            foreach (var obj1 in objects1)
            {
                foreach (var obj2 in objects2)
                {
                    if (obj1.GetComponent<CollisionEventComponent>()==null)
                    {
                        obj1.AddComponent<CollisionEventComponent>();
                    }
                    obj1.GetComponent<CollisionEventComponent>().AddTarget(obj2,action);
                    if (obj2.GetComponent<CollisionEventComponent>()==null)
                    {
                        obj2.AddComponent<CollisionEventComponent>();
                    }
                    obj2.GetComponent<CollisionEventComponent>().AddTarget(obj1,action);
                }
            }
        }
    }
}