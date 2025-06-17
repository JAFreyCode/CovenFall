using UnityEngine;

namespace NSG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Base Weapon Action")]
    public class BaseWeaponAction : WeaponItemAction
    {
        [SerializeField] string base_Attack_01 = "Main_Base_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.isDead.Value)
                return;

            if (!playerPerformingAction.isGrounded)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            PerformBaseWeaponAction(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformBaseWeaponAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(base_Attack_01, true);
                return;
            }

            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                return;
            }
        }
    }
}
