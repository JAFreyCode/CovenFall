using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NSG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Health Settings")]
        public int healthStartingIncreaseAmount = 45;
        public int minimumHealthIncreaseAmount = 10;

        [Header("Stamina Settings")]
        public int staminaStartingIncreaseAmount = 15;
        public int minimumStaminaIncreaseAmount = 1;

        [Header("Stamina Rengeration Settings")]
        public float staminaRengerationTickSpeed = 0.01f;
        public float staminaRegenerationDelay = 1;
        public float staminaRegenerationAmount = 15;

        [Header("Stamina Regeneration Data")]
        public float staminaRegenerationTimer = 0;
        public float staminaTickTimer = 0;

        protected virtual void Awake()
        {
            GetReferences();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public float CalculateHealthBaseOnVigorLevel(int vigor)
        {
            float health = 0;

            for (int level = 1; level <= vigor; level++)
            {
                float D = healthStartingIncreaseAmount * (level * 0.01f);
                float gain = Mathf.Max(healthStartingIncreaseAmount - D, minimumHealthIncreaseAmount);

                health += gain;

                health = Mathf.Round(health);
            }

            return health;
        }

        public float CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            for (int level = 1; level <= endurance; level++)
            {
                float D = staminaStartingIncreaseAmount * (level * 0.01f);
                float gain = Mathf.Max(staminaStartingIncreaseAmount - D, minimumStaminaIncreaseAmount);

                stamina += gain;

                stamina = Mathf.Round(stamina);
            }

            return stamina;
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner) return;

            if (character.isDead.Value) return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer < staminaRegenerationDelay) return;

            if (character.characterNetworkManager.currentStamina.Value >= character.characterNetworkManager.maxStamina.Value) return;

            staminaTickTimer += Time.deltaTime;

            if (staminaTickTimer < staminaRengerationTickSpeed) return;

            staminaTickTimer = 0;
            character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
        }

        protected virtual void GetReferences()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
