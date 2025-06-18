using Unity.Netcode;
using UnityEngine;

namespace NSG
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public float networkPositionSmoothTime = 0.1f;

        [Header("Rotation")]
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public float networkRotationSmoothTime = 0.1f;

        [Header("Flags")]
        public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Animator")]
        public NetworkVariable<float> networkHorizontal = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkVertical = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkAbs_MoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Vigor Stats")]
        public NetworkVariable<int> vigor = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> maxHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Endurance Stats")]
        public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> maxStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Network Velocity")]
        public Vector3 networkPositionVelocity;


        protected virtual void Awake()
        {
            GetReferences();
        }

        public void CheckHealth(float oldHealth, float newHealth)
        {
            if (currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }

            // PREVENTS US FROM OVER HEALING
            if (character.IsOwner)
            {
                if (currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }

        protected virtual void GetReferences()
        {
            character = GetComponent<CharacterManager>();
        }

        // ANIMATION SERVER NOTIFY

        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (!IsServer) return;

            PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);

        }

        [ClientRpc]
        private void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (clientID == NetworkManager.Singleton.LocalClientId) return;

            PlayActionAnimationForAllClients(animationID, applyRootMotion);
        }

        private void PlayActionAnimationForAllClients(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, character.characterAnimatorManager.crossFadeAnimationSmoothing);
        }

        // ATTACK ANIMATION SERVER NOTIFY

        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (!IsServer) return;

            PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);

        }

        [ClientRpc]
        private void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (clientID == NetworkManager.Singleton.LocalClientId) return;

            PlayAttackActionAnimationForAllClients(animationID, applyRootMotion);
        }

        private void PlayAttackActionAnimationForAllClients(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, character.characterAnimatorManager.crossFadeAnimationSmoothing);
        }

        // ATTACK DAMAGE SERVER NOTIFY

        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float fireMagicDamage,
            float lightningMagicDamage,
            float corruptionMagicDamage,
            float iceMagicDamage,
            float poisonMagicDamage,
            float blackDeathMagicDamage,
            float bloodMagicDamage,
            float madnessMagicDamage,
            float holyMagicDamage,
            float demonMagicDamage,
            float arcaneMagicDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
            )
        {
            if (IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(
                    damagedCharacterID, 
                    characterCausingDamageID, 
                    physicalDamage, 
                    fireMagicDamage, 
                    lightningMagicDamage, 
                    corruptionMagicDamage, 
                    iceMagicDamage, 
                    poisonMagicDamage, 
                    blackDeathMagicDamage, 
                    bloodMagicDamage, 
                    madnessMagicDamage, 
                    holyMagicDamage, 
                    demonMagicDamage, 
                    arcaneMagicDamage, 
                    angleHitFrom, 
                    contactPointX, 
                    contactPointY, 
                    contactPointZ 
                    );
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float fireMagicDamage,
            float lightningMagicDamage,
            float corruptionMagicDamage,
            float iceMagicDamage,
            float poisonMagicDamage,
            float blackDeathMagicDamage,
            float bloodMagicDamage,
            float madnessMagicDamage,
            float holyMagicDamage,
            float demonMagicDamage,
            float arcaneMagicDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
            )
        {
            ProcessCharacterDamageFromServer(
                damagedCharacterID, 
                characterCausingDamageID, 
                physicalDamage, 
                fireMagicDamage, 
                lightningMagicDamage, 
                corruptionMagicDamage, 
                iceMagicDamage, 
                poisonMagicDamage, 
                blackDeathMagicDamage, 
                bloodMagicDamage, 
                madnessMagicDamage, 
                holyMagicDamage, 
                demonMagicDamage, 
                arcaneMagicDamage, 
                angleHitFrom, 
                contactPointX, 
                contactPointY, 
                contactPointZ 
                );
        }

        private void ProcessCharacterDamageFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float fireMagicDamage,
            float lightningMagicDamage,
            float corruptionMagicDamage,
            float iceMagicDamage,
            float poisonMagicDamage,
            float blackDeathMagicDamage,
            float bloodMagicDamage,
            float madnessMagicDamage,
            float holyMagicDamage,
            float demonMagicDamage,
            float arcaneMagicDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
            )
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();

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

            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}
