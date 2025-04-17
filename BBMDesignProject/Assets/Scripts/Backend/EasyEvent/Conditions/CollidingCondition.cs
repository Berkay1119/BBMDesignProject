using System;
using Backend.Attributes;
using Backend.Components;
using Backend.Object;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Backend.Conditions
{
    [Condition]
    public class CollidingCondition:EasyCondition
    {
        public EasyObject _objectOne;
        public EasyObject _objectTwo;
        
        private Collider2D firstCollider;
        private Collider2D secondCollider;
        
        public CollidingCondition()
        {
            conditionName = "CollidingCondition";
            conditionDescription = "Check if two objects are colliding";
        }

        public override void DrawGUI()
        {
            base.DrawGUI();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Object One:", GUILayout.Width(150));
            _objectOne= (EasyObject)EditorGUILayout.ObjectField(_objectOne, typeof(EasyObject), true);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Object Two:", GUILayout.Width(150));
            _objectTwo= (EasyObject)EditorGUILayout.ObjectField(_objectTwo, typeof(EasyObject), true);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public override void Setup(EasyEvent easyEvent)
        {
            base.Setup(easyEvent);
            
            if (_objectOne.GetComponent<PolygonCollider2D>()==null)
            {
                firstCollider=_objectOne.AddComponent<PolygonCollider2D>();
            }
            else
            {
                firstCollider=_objectOne.GetComponent<PolygonCollider2D>();
            }
            
            
            if (_objectTwo.GetComponent<PolygonCollider2D>()==null)
            {
                secondCollider=_objectTwo.AddComponent<PolygonCollider2D>();
            }
            else
            {
                secondCollider=_objectTwo.GetComponent<PolygonCollider2D>();
            }
            
        }

        public override bool Check()
        {
            if (firstCollider == null || secondCollider == null)
            {
                return false;
            }
            return firstCollider.IsTouching(secondCollider);
        }
    }
}