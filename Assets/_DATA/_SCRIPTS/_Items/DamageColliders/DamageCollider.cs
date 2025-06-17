using System.Collections.Generic;
using UnityEngine;

namespace NSG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        protected Collider damageCollider;

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

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if(damageTarget != null )
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE | ONLY IMPLEMENTED BECAUSE OF KICK

                // CHECK IF TARGET IS BLOCKING

                // CHECK IF TARGET IS INVULNERABLE

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            InstantDamageEffect instantDamageEffect = Instantiate(WorldEffectsManager._Singleton.instantDamageEffect);

            instantDamageEffect.physicalDamage = physicalDamage;
            instantDamageEffect.fireMagicDamage = fireMagicDamage;
            instantDamageEffect.lightningMagicDamage = lightningMagicDamage;
            instantDamageEffect.corruptionMagicDamage = corruptionMagicDamage;
            instantDamageEffect.iceMagicDamage = iceMagicDamage;
            instantDamageEffect.poisonMagicDamage = poisonMagicDamage;
            instantDamageEffect.blackDeathMagicDamage = blackDeathMagicDamage;
            instantDamageEffect.bloodMagicDamage = bloodMagicDamage;
            instantDamageEffect.madnessMagicDamage = madnessMagicDamage;
            instantDamageEffect.holyMagicDamage = holyMagicDamage;
            instantDamageEffect.demonMagicDamage = demonMagicDamage;
            instantDamageEffect.arcaneMagicDamage = arcaneMagicDamage;

            instantDamageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(instantDamageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();
        }
    }
}
