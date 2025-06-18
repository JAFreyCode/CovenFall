using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class PlayerManager : CharacterManager
    {
        [Header("DEBUG")]
        [SerializeField] bool switchRightWeapon = false;
        [SerializeField] bool switchLeftWeapon = false;

        [Header("References")]
        public PlayerLocomotionManager playerLocomotionManager {  get; private set; }
        public PlayerNetworkManager playerNetworkManager { get; private set; }
        public PlayerAnimatorManager playerAnimatorManager { get; private set; }
        public PlayerSFXManager playerSFXManager { get; private set; }
        public PlayerStatsManager playerStatsManager { get; private set; }
        public PlayerEffectsManager playerEffectsManager { get; private set; }
        public PlayerInventoryManager playerInventoryManager { get; private set; }
        public PlayerEquipmentManager playerEquipmentManager { get; private set; }
        public PlayerCombatManager playerCombatManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerSFXManager = GetComponent<PlayerSFXManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
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

            Debug();
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

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            if (IsOwner)
            {
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

            // NETWORK DEATH STATE
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHealth;

            // EQUIPMENT NETWORK VARIABLES
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

            // THIS DOESNT RUN IF WE ARE THE HOST
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager._Singleton.currentCharacterData);
            }
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            GameSessionManager._Singleton.AddPlayerToActivePlayersList(this);
            
            // IF WE ARE THE HOST WE DONT NEED TO SYNC OTHER PLAYERS
            // ONLY JOINING CLIENTS HAVE TO SYNC
            if (!IsServer && IsOwner)
            {
                foreach (var player in GameSessionManager._Singleton.players)
                {
                    if (player != this)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManager._Singleton.playerUIPopUpManager.SendYouHaveBeenCursedPopUp();
            }

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
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
            currentCharacterData.vigorLevel = playerNetworkManager.vigor.Value;
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
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

        public void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            // SYNC WEAPONS
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);

            // ARMOR
        }

        public override void RevivePlayer()
        {
            base.RevivePlayer();

            if (IsOwner)
            {
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
                // RESTORE FOCUS POINTS

                // REBIRTH EFFECTS

                playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            }
        }

        private void Debug()
        {
            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }

            if (switchLeftWeapon)
            {
                switchLeftWeapon = false;
                playerEquipmentManager.SwitchLeftWeapon();
            }
        }
    }
}
