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
    }
}
