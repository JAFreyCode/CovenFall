using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

namespace NSG
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Character Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("References")]
        public CharacterLocomotionManager characterLocomotionManager {  get; private set; }
        public CharacterNetworkManager characterNetworkManager { get; private set; }
        public CharacterAnimatorManager characterAnimatorManager { get; private set; }
        public CharacterSFXManager characterSFXManager { get; private set; }
        public CharacterStatsManager characterStatsManager { get; private set; }
        public CharacterEffectsManager characterEffectsManager { get; private set; }
        public CharacterCombatManager characterCombatManager { get; private set; }

        [Header("Component References")]
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool canMove = true;
        public bool canRotate = true;
        public bool isGrounded = false;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterSFXManager = GetComponent<CharacterSFXManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }

        protected virtual void Start()
        {
            DontDestroyOnLoad(this);

            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // RESET AMY FLAGS HERE THAT NEED TO BE RESET
                

                if (manuallySelectDeathAnimation) { /* manual animation here */ yield return null; }

                if (isGrounded)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
                else
                {
                    // SAME ANIMATION FOR NOW
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }

            // PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5);

            // AWARD PLAYERS WITH RUNES

            // DISABLE CHARACTER
        }

        public virtual void RevivePlayer()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            ignoreColliders.Add(characterControllerCollider);

            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider);
                }
            }
        }
    }
}
