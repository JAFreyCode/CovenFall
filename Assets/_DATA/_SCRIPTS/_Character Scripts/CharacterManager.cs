using UnityEngine;
using Unity.Netcode;

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

        [Header("Component References")]
        public CharacterController characterController { get; private set; }
        public Animator animator { get; private set; }

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool canMove = true;
        public bool canRotate = true;
        public bool isGrounded = false;
        public bool isJumping = false;

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            DontDestroyOnLoad(this);
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

        protected virtual void GetReferences()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterSFXManager = GetComponent<CharacterSFXManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
        }
    }
}
