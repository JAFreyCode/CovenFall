using UnityEngine;

namespace NSG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            player.playerNetworkManager.maxHealth.Value = CalculateHealthBaseOnVigorLevel(player.playerNetworkManager.vigor.Value);
            player.playerNetworkManager.maxStamina.Value = CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);

            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
