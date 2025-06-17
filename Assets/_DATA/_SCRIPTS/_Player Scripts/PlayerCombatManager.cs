using UnityEngine;

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
            // PERFORM THE ACTION
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // ALSO PERFORM THE ACTION ON OTHER CLIENTS / NOTIFY THE SERVER TO RUN THIS ON OTHER CLIENTS

        }
    }
}
