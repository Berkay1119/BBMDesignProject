using Backend.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Backend.Actions
{
    [Action]
    public class ActivateObjectAction:EasyAction
    {
        public GameObject GameObjectToActivate;
        
        public ActivateObjectAction()
        {
            actionName = "Activate Object";
            actionDescription = "Activates a game object";
        }
        
        public override void DrawGUI()
        {
            base.DrawGUI();
            GameObjectToActivate = (GameObject)UnityEditor.EditorGUILayout.ObjectField("Object", GameObjectToActivate, typeof(GameObject), true);
            GUILayout.EndVertical();
        }
        
        public override void Execute()
        {
            GameObjectToActivate.SetActive(true);
        }
    }
}