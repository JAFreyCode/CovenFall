using UnityEngine;

namespace NSG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Current Held Weapons")]
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;
    }
}
