using Backend.Attributes;
using Backend.Components.SubComponents;
using UnityEngine;

namespace Backend.Components
{
    [Component]
    public class CharacterComponent: BaseComponent
    {
        [SerializeField] public bool cameraFollowCharacter = false;
        [SerializeField] private string leftKey = "a";
        [SerializeField] private string rightKey = "d";
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 10f;
        public char LeftKey => leftKey[0];
        public char RightKey => rightKey[0];
        public float Speed => speed;
        public float JumpForce => jumpForce;
        
        private PlayerControllerComponent _playerControllerComponent;
        private Rigidbody2D _playerRigidbody2D;
        
        public CharacterComponent()
        {
            SetName("Character");
            SetDescription("This component is used to define a character in the game.");
        }

        protected override void OnEnable() {
            base.OnEnable();
            _playerControllerComponent=gameObject.AddComponent<PlayerControllerComponent>();
            _addedComponents.Add(_playerControllerComponent);
            var tempCollider =gameObject.AddComponent<BoxCollider2D>();
            _addedComponents.Add(tempCollider);
            _playerRigidbody2D=gameObject.AddComponent<Rigidbody2D>();
            _addedComponents.Add(_playerRigidbody2D);
            _playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            _playerControllerComponent.Setup(this,_playerRigidbody2D);
            
            if (!cameraFollowCharacter) {
                return;
            }

            var cameraFollow = Camera.main.gameObject.AddComponent<CameraFollowComponent>();
            cameraFollow.target = gameObject.transform;

        }

        public override void SetupComponent()
        {
            gameObject.tag = "Character";
            gameObject.layer = 7;
        }
    }
}