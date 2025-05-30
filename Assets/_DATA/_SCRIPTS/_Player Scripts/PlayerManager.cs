using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class PlayerManager : CharacterManager
    {
        [Header("References")]
        public PlayerLocomotionManager playerLocomotionManager {  get; private set; }
        public PlayerNetworkManager playerNetworkManager { get; private set; }
        public PlayerAnimatorManager playerAnimatorManager { get; private set; }
        public PlayerSFXManager playerSFXManager { get; private set; }
        public PlayerStatsManager playerStatsManager { get; private set; }
        public PlayerEffectsManager playerEffectsManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
        }

        protected override void Start()
        {
            base.Start();

        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;

            playerLocomotionManager.HandleAllLocomotion();

            playerStatsManager.RegenerateStamina();
        }

        protected override void FixedUpdate()
        {
            if (!IsOwner)
                return;

            base.FixedUpdate();

        }

        protected override void LateUpdate()
        {
            if (!IsOwner) 
                return;

            base.LateUpdate();

            PlayerCameraManager._Singleton.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                return;

            AssignLocalPlayer();

            // HEALTH STAT
            playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager._Singleton.playerUIHudManager.SetNewHealthValue;

            // STAMINA STAT
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager._Singleton.playerUIHudManager.SetNewStaminaValue;

            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

        }

        private void AssignLocalPlayer()
        {
            PlayerCameraManager._Singleton.player = this;
            WorldInputManager._Singleton.player = this;
            CommandConsoleManager._Singleton.player = this;
            WorldSaveGameManager._Singleton.player = this;
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = 1;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.enduranceLevel = playerNetworkManager.endurance.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.worldPositionX = transform.position.x;
            currentCharacterData.worldPositionY = transform.position.y;
            currentCharacterData.worldPositionZ = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            playerNetworkManager.vigor.Value = currentCharacterData.vigorLevel;
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.endurance.Value = currentCharacterData.enduranceLevel;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            transform.position = new Vector3(currentCharacterData.worldPositionX, currentCharacterData.worldPositionY, currentCharacterData.worldPositionZ);

            // HEALTH
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBaseOnVigorLevel(currentCharacterData.vigorLevel);
            PlayerUIManager._Singleton.playerUIHudManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);

            // STAMINA
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.enduranceLevel);
            PlayerUIManager._Singleton.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerSFXManager = GetComponent<PlayerSFXManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }
    }
}
