using UnityEngine;

namespace NSG
{
    public class WeaponItem : Item
    {
        // ANIMATOR CONTROLLER OVERRIDE (Change Attack Animations Based On Weapon You Are Currently Using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexterityREQ = 0;
        public int intelligenceREQ = 0;
        public int faithREQ = 0;
        public int arcaneREQ = 0;
        public int darkArtsREQ = 0;
        public int botanyREQ = 0;

        [Header("Weapon Base Damage")]
        public float physicalDamage; /* Split in more sub types | Blunt, Pierce, Bleed, effective */
        public float fireMagicDamage;
        public float lightningMagicDamage;
        public float corruptionMagicDamage;
        public float iceMagicDamage;
        public float poisonMagicDamage;
        public float blackDeathMagicDamage;
        public float bloodMagicDamage;
        public float madnessMagicDamage;
        public float holyMagicDamage;
        public float demonMagicDamage;
        public float arcaneMagicDamage;

        // WEAPON GUARD ABSORPTIONS

        [Header("Weapon Base Poise")]
        public float poise = 10;
        // OFFSENSIVE POISE BONUS

        [Header("Attack Modifiers")]
        public float base_Attack_01_Modifier = 1;
        // HEAVY ATTACK MODIFIER
        // CRITICAL DAMAGE MODIFIER

        [Header("Stamina Cost Modifiers")]
        public int baseStaminaCost = 20;
        public float baseAttackStaminaMultiplier = 1;
        // RUNNING ATTACK STAMINA COST MODIFIER
        // LIGHT ATTACK STAMINA COST MODIFIER
        // HEAVY ATTACK STAMINA COST MODIFIER

        [Header("Actions")]
        public WeaponItemAction oh_baseAction;
        public WeaponItemAction oh_enchancedAction;
        public WeaponItemAction oh_chargedAction;

        // CURSES / ENCHANTATIONS

        // BLOCKING SOUNDS
    }
}
