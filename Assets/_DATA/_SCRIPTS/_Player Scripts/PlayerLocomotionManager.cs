using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NSG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("References")]
        PlayerManager player;

        [Header("Locomotion Settings")]
        public float walkingSpeed = 2;
        public float runningSpeed = 5;
        public float sprintingSpeed = 7;
        private float currentMovementSpeed;
        private float currentJumpHeight;
        private float currentJumpDistance;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] Vector3 rollDirection;

        [Header("Jump Settings")]
        public float idleJumpHeight = 1;
        public float walkingJumpHeight = 2;
        public float runningJumpHeight = 2;
        public float sprintingJumpHeight = 2;
        public float idleJumpDistance;
        public float walkingJumpDistance;
        public float runningJumpDistance;
        public float sprintingJumpDistance;

        [Header("Locomotion Stamina Settings")]
        public float dodgeStaminaCost = 26;
        public float sprintStaminaCost = 2;
        public float jumpStaminaCost = 10;

        [Header("Locomotion Values")]
        public Vector3 moveDirection;
        public Vector3 targetRotationDirection;
        public Vector3 jumpDirection;

        InstantStaminaDrainEffect effect;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            effect = ScriptableObject.CreateInstance<InstantStaminaDrainEffect>();
        }

        protected override void Update()
        {
            base.Update();

            UpdatePlayerNetworkAnimatorValues();
        }

        public void HandleAllLocomotion()
        {
            float verticalMovement = WorldInputManager._Singleton.vertical_Input;
            float horizontalMovement = WorldInputManager._Singleton.horizontal_Input;
            float absMovement = WorldInputManager._Singleton.absMove_Input;

            HandleGroundedLocomotion(verticalMovement, horizontalMovement, absMovement);
            HandleRotation(verticalMovement, horizontalMovement);
            HandleJumpingMovement();
        }

        private void HandleGroundedLocomotion(float verticalMovement, float horizontalMovement, float absMovement)
        {
            if (player.isPerformingAction) return;

            if (!player.isGrounded) return;

            if (!player.canMove) return;

            moveDirection = PlayerCameraManager._Singleton.pivotLeftAndRight.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCameraManager._Singleton.pivotLeftAndRight.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (WorldInputManager._Singleton.absMove_Input > 1f && player.playerNetworkManager.currentStamina.Value > 0)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);

                ApplyStaminaEffect(sprintStaminaCost * Time.deltaTime);
                currentJumpHeight = sprintingJumpHeight;
                currentJumpDistance = sprintingJumpDistance;
                currentMovementSpeed = sprintingSpeed;
            }
            else if (WorldInputManager._Singleton.absMove_Input > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                currentJumpHeight = runningJumpHeight;
                currentJumpDistance = runningJumpDistance;
                currentMovementSpeed = runningSpeed;
            }
            else if (WorldInputManager._Singleton.absMove_Input <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                currentJumpHeight = walkingJumpHeight;
                currentJumpDistance = walkingJumpDistance;
                currentMovementSpeed = walkingSpeed;
            }
            else
            {
                currentJumpHeight = idleJumpHeight;
                currentJumpDistance = idleJumpDistance;
                currentMovementSpeed = 0;
            }
        }

        private void HandleJumpingMovement()
        {
            if (!player.isJumping) return;

            player.characterController.Move(jumpDirection * currentMovementSpeed * Time.deltaTime);
        }

        private void HandleRotation(float verticalMovement, float horizontalMovement)
        {
            if (!player.canRotate) return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCameraManager._Singleton.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCameraManager._Singleton.cameraObject.transform.right * horizontalMovement;

            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
                targetRotationDirection = transform.forward;

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }

        private void UpdatePlayerNetworkAnimatorValues()
        {
            if (player.IsOwner)
            {
                player.characterNetworkManager.networkHorizontal.Value = WorldInputManager._Singleton.horizontal_Input;
                player.characterNetworkManager.networkVertical.Value = WorldInputManager._Singleton.vertical_Input;
                player.characterNetworkManager.networkAbs_MoveAmount.Value = WorldInputManager._Singleton.absMove_Input;
            }
            else
            {
                float horizontalAmount = player.characterNetworkManager.networkHorizontal.Value;
                float verticalAmount = player.characterNetworkManager.networkVertical.Value;
                float abs_MoveAmount = player.characterNetworkManager.networkAbs_MoveAmount.Value;

                player.playerAnimatorManager.UpdateAnimatorMovementParemeters(0, abs_MoveAmount);
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction) return;

            if (!player.isGrounded) return;

            if (player.playerNetworkManager.currentStamina.Value <= 0) return;

            // IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, WE PERFORM A ROLL
            if (WorldInputManager._Singleton.absMove_Input > 0)
            {
                rollDirection = PlayerCameraManager._Singleton.cameraObject.transform.forward * WorldInputManager._Singleton.vertical_Input;
                rollDirection += PlayerCameraManager._Singleton.cameraObject.transform.right * WorldInputManager._Singleton.horizontal_Input;

                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                // PLAY PLAYER ROLL ANIMATION
                player.playerAnimatorManager.PlayTargetActionAnimation(player.playerAnimatorManager.rollAnimation, true);

                ApplyStaminaEffect(dodgeStaminaCost);
            }
            // IF WE ARE STATIONARY, WE PERFORM A BACKSTEP
            else
            {
                // PLAY PLAYER BACKSTEP ANIMATION
                player.playerAnimatorManager.PlayTargetActionAnimation(player.playerAnimatorManager.backStepAnimation, true);

                ApplyStaminaEffect(dodgeStaminaCost);
            }
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) return;

            if (player.playerNetworkManager.currentStamina.Value <= 0) return;

            if (player.isJumping) return;

            if (!player.isGrounded) return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_Start", false);

            player.isJumping = true;

            ApplyStaminaEffect(jumpStaminaCost);

            jumpDirection = PlayerCameraManager._Singleton.cameraObject.transform.forward * WorldInputManager._Singleton.vertical_Input;
            jumpDirection += PlayerCameraManager._Singleton.cameraObject.transform.right * WorldInputManager._Singleton.horizontal_Input;
            jumpDirection.y = 0;

            if (jumpDirection == Vector3.zero) return;

            jumpDirection *= currentJumpDistance;
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(currentJumpHeight * -2 * gravityForce);
        }

        public void PlayRollSoundEffect()
        {
            player.playerSFXManager.PlaySFX(WorldSFXManager._Singleton.rollSFX, player.playerSFXManager.playerSoundEffectsVolume);
        }

        public void PlayBackstepSoundEffect()
        {
            player.playerSFXManager.PlaySFX(WorldSFXManager._Singleton.backStepSFX, player.playerSFXManager.playerSoundEffectsVolume);
        }

        private void ApplyStaminaEffect(float staminaUsage)
        {
            effect.staminaDamage = staminaUsage;
            player.playerEffectsManager.ProcessInstantEffect(effect);
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            player = GetComponent<PlayerManager>();
        }
    }
}
