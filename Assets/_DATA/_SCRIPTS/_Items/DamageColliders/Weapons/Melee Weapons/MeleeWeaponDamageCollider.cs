using UnityEngine;

namespace NSG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

        [Header("Weapon Attack Modifiers")]
        public float base_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }

            damageCollider.enabled = false; // DISABLE ON STARTUP JUST INCASE, SHOULD ONLY BE ENABLED DURING A ATTACK ANIMATION
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            Debug.Log(damageTarget);

            if (damageTarget == characterCausingDamage)
                return;

            if (damageTarget.isDead.Value)
                return;

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            InstantDamageEffect damageEffect = Instantiate(WorldEffectsManager._Singleton.instantDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.fireMagicDamage = fireMagicDamage;
            damageEffect.lightningMagicDamage = lightningMagicDamage;
            damageEffect.corruptionMagicDamage = corruptionMagicDamage;
            damageEffect.iceMagicDamage = iceMagicDamage;
            damageEffect.poisonMagicDamage = poisonMagicDamage;
            damageEffect.blackDeathMagicDamage = blackDeathMagicDamage;
            damageEffect.bloodMagicDamage = bloodMagicDamage;
            damageEffect.madnessMagicDamage = madnessMagicDamage;
            damageEffect.holyMagicDamage = holyMagicDamage;
            damageEffect.demonMagicDamage = demonMagicDamage;
            damageEffect.arcaneMagicDamage = arcaneMagicDamage;

            damageEffect.contactPoint = contactPoint;

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.BaseAttack01:
                    ApplyAttackDamageModifiers(base_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            //damageTarget.characterEffectsManager.ProcessInstantEffect(instantDamageEffect);

            if (characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId, 
                    characterCausingDamage.NetworkObjectId, 
                    damageEffect.physicalDamage, 
                    damageEffect.fireMagicDamage, 
                    damageEffect.lightningMagicDamage, 
                    damageEffect.corruptionMagicDamage, 
                    damageEffect.iceMagicDamage, 
                    damageEffect.poisonMagicDamage, 
                    damageEffect.blackDeathMagicDamage, 
                    damageEffect.bloodMagicDamage, 
                    damageEffect.madnessMagicDamage, 
                    damageEffect.holyMagicDamage, 
                    damageEffect.demonMagicDamage, 
                    damageEffect.arcaneMagicDamage, 
                    damageEffect.angleHitFrom, 
                    damageEffect.contactPoint.x, 
                    damageEffect.contactPoint.y, 
                    damageEffect.contactPoint.z
                    );
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, InstantDamageEffect damageEffect)
        {
            damageEffect.physicalDamage *= modifier;
            damageEffect.fireMagicDamage *= modifier;
            damageEffect.lightningMagicDamage *= modifier;
            damageEffect.corruptionMagicDamage *= modifier;
            damageEffect.iceMagicDamage *= modifier;
            damageEffect.blackDeathMagicDamage *= modifier;
            damageEffect.bloodMagicDamage *= modifier;
            damageEffect.madnessMagicDamage *= modifier;
            damageEffect.holyMagicDamage *= modifier;
            damageEffect.demonMagicDamage *= modifier;
            damageEffect.arcaneMagicDamage *= modifier;
            damageEffect.poiseDamage *= modifier;
        }
    }
}
