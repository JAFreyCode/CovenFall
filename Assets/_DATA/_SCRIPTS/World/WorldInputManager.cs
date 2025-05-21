using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class WorldInputManager : MonoBehaviour
    {
        private static WorldInputManager Singleton;
        public static WorldInputManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("References")]
        public PlayerManager player;

        [Header("Player Inputs")]
        public Vector2 movement_Input { get; private set; }
        public Vector2 camera_Input { get; private set; }

        [Header("Player Movement Values")]
        public float horizontal_Input { get; private set; }
        public float vertical_Input { get; private set; }
        public float absMove_Input { get; private set; }
        public bool dodge_Input { get; private set; }
        public bool forceWalk_Input { get; private set; }
        public bool sprint_Input { get; private set; }
        public bool jump_Input { get; private set; }

        [Header("Camera Movement Values")]
        public float cameraHorizontal_Input { get; private set; }
        public float cameraVertical_Input { get; private set; }

        PlayerControls playerControls;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void OnEnable()
        {
            if (playerControls == null) { playerControls = new PlayerControls(); }

            // PLAYER MOVEMENT CONTROLS
            playerControls.PlayerMovement.Movement.performed += i => movement_Input = i.ReadValue<Vector2>(); // PLAYER MOVEMENT

            // DODGE - ROLL/BACKSTEP
            playerControls.PlayerMovement.Dodge.performed += i => dodge_Input = true;

            // FORCE WALK
            playerControls.PlayerMovement.ForceWalk.performed += i => forceWalk_Input = true;
            playerControls.PlayerMovement.ForceWalk.canceled += i => forceWalk_Input = false;

            // SPRINT
            playerControls.PlayerMovement.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprint_Input = false;

            // JUMP
            playerControls.PlayerMovement.Jump.performed += i => jump_Input = true;

            // CAMERA MOVEMENT CONTROLS
            playerControls.CameraMovement.Movement.performed += i => camera_Input = i.ReadValue<Vector2>(); // CAMERA MOVEMENT

            playerControls.Enable();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            Singleton.enabled = false;
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!enabled)
                return;

            if (focus)
                playerControls.Enable();
            else
                playerControls.Disable();
        }

        private void HandleAllInputs()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleJumpInput();
        }

        private void HandleMovementInput()
        {
            if (player == null) { return; }

            movement_Input.Normalize();

            vertical_Input = movement_Input.y;
            horizontal_Input = movement_Input.x;

            absMove_Input = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

            if (sprint_Input && absMove_Input > 0 && player.playerNetworkManager.currentStamina.Value > 0)
            {
                absMove_Input = 2f;
            }
            else if (forceWalk_Input && absMove_Input > 0)
            {
                absMove_Input = 0.5f;
            }
            else
            {
                if (absMove_Input <= 0.5 && absMove_Input > 0)
                    absMove_Input = 0.5f;
                else if (absMove_Input > 0.5 && absMove_Input <= 1)
                    absMove_Input = 1;
            }

            player.playerAnimatorManager.UpdateAnimatorMovementParemeters(0, absMove_Input);
        }

        private void HandleCameraMovementInput()
        {
            cameraVertical_Input = camera_Input.y;
            cameraHorizontal_Input = camera_Input.x;
        }

        private void HandleDodgeInput()
        {
            if (!dodge_Input) return;

            dodge_Input = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }

        private void HandleJumpInput()
        {
            if (jump_Input)
            {
                jump_Input = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex != NSGUtils.GetWorldSceneIndex(true)) { Singleton.enabled = false; return; }

            Singleton.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
