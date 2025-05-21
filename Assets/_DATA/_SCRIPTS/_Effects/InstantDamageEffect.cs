using UnityEngine;

namespace NSG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/InstantDamageEffect")]
    public class InstantDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage Types")]
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

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSFX; /* USE FOR ELEMENTAL SOUNDS FOR MAGICAL OR ENCHANTED WEAPONS */

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;

        [Header("Damage Data")]
        private float finalDamageDealt = 0;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
        }
    }
}
