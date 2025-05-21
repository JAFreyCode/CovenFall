using UnityEngine;

namespace NSG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/InstantStaminaDrainEffect")]
    public class InstantStaminaDrainEffect : InstantCharacterEffect
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                if (character.characterNetworkManager.currentStamina.Value <= 0) return;

                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
                character.characterStatsManager.staminaRegenerationTimer = 0;
            }
        }
    }
}
