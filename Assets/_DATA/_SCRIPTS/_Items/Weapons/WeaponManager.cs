using UnityEngine;

namespace NSG
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.fireMagicDamage = weapon.fireMagicDamage;
            meleeDamageCollider.lightningMagicDamage = weapon.lightningMagicDamage;
            meleeDamageCollider.corruptionMagicDamage = weapon.corruptionMagicDamage;
            meleeDamageCollider.iceMagicDamage = weapon.iceMagicDamage;
            meleeDamageCollider.poisonMagicDamage = weapon.poisonMagicDamage;
            meleeDamageCollider.blackDeathMagicDamage = weapon.blackDeathMagicDamage;
            meleeDamageCollider.bloodMagicDamage = weapon.bloodMagicDamage;
            meleeDamageCollider.madnessMagicDamage = weapon.madnessMagicDamage;
            meleeDamageCollider.holyMagicDamage = weapon.holyMagicDamage;
            meleeDamageCollider.demonMagicDamage = weapon.demonMagicDamage;
            meleeDamageCollider.arcaneMagicDamage = weapon.arcaneMagicDamage;

            meleeDamageCollider.base_Attack_01_Modifier = weapon.base_Attack_01_Modifier;
        }
    }
}
