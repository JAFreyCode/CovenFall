using UnityEngine;
using Unity.Netcode;

namespace NSG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        [Header("References")]
        CharacterManager character;

        [Header("Animator Data")]
        int vertical;
        int horizontal;

        [Header("Animation Movement Settings")]
        public float animationSmoothing = 0.1f;
        public float crossFadeAnimationSmoothing = 0.2f;

        [Header("Character Action Animations")]
        public string rollAnimation = "Roll_Forward_01";
        public string backStepAnimation = "Back_Step_01";

        [Header("Character Flags")]
        public bool applyRootMotion = false;

        protected virtual void Awake()
        {
            GetReferences();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void Update()
        {

        }

        public void UpdateAnimatorMovementParemeters(float horizontalValue, float verticalValue)
        {
            if (character == null) { return; }

            if (character.animator == null) { return; }

            character.animator.SetFloat(horizontal, horizontalValue, animationSmoothing, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalValue, animationSmoothing, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false
            )
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, crossFadeAnimationSmoothing);
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false
            )
        {
            character.characterCombatManager.currentAttackType = attackType;
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, crossFadeAnimationSmoothing);
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        protected virtual void GetReferences()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
