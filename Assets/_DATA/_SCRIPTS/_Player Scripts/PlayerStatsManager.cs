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
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
