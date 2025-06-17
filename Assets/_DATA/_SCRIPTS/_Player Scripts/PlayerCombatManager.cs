using UnityEngine;
using Unity.Netcode;

namespace NSG
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        public override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                // PERFORM THE ACTION
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

                // ALSO PERFORM THE ACTION ON OTHER CLIENTS / NOTIFY THE SERVER TO RUN THIS ON OTHER CLIENTS
                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;

            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0;

            switch (currentAttackType)
            {
                case AttackType.BaseAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.baseAttackStaminaMultiplier;
                    break;
                default:
                    break;
            }

            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
            player.playerStatsManager.staminaRegenerationTimer = 0;
        }
    }
}
