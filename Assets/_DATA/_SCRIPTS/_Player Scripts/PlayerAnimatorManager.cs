using UnityEngine;

namespace NSG
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void OnAnimatorMove()
        {
            if (!applyRootMotion) return;

            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            player = GetComponent<PlayerManager>();
        }
    }
}
